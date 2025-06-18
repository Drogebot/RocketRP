using RocketRP.Actors.Core;
using RocketRP.Actors.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RocketRP
{
	public class ActorUpdate
	{
		public int ChannelId { get; set; }
		public ChannelState State { get; set; }
		public int? NameId { get; set; }
		public string? Name { get; set; }
		public ObjectTarget<ClassObject> TypeId { get; set; }
		public string TypeName { get; set; } = null!;
		public int ObjectId { get; set; }
		public string ObjectName { get; set; } = null!;
		public Type Type { get; set; } = null!;
		public Actor Actor { get; set; } = null!;

		private ClassNetCache ClassNetCache = null!;
		//public HashSet<int> SetPropertyObjectIndexes;
		//public HashSet<string> SetPropertyNames;

		public static ActorUpdate Deserialize(BitReader br, Replay replay, Dictionary<int, ActorUpdate> openChannels)
		{
			var actorUpdate = new ActorUpdate();

			actorUpdate.ChannelId = br.ReadInt32((uint)replay.MaxChannels);

			if (br.ReadBit())
			{
				if (br.ReadBit())
				{
					actorUpdate.State = ChannelState.Open;

					if ((replay.EngineVersion >= 868 && replay.LicenseeVersion >= 15) ||
						(replay.EngineVersion == 868 && replay.LicenseeVersion == 14 && replay.Properties.MatchType != "Lan"))  // Fixes RLCS 2 replays
					{
						actorUpdate.NameId = br.ReadInt32();
						actorUpdate.Name = replay.Names[actorUpdate.NameId.Value];
					}

					actorUpdate.TypeId = ObjectTarget<ClassObject>.Deserialize(br);
					if (actorUpdate.TypeId.IsActor)
					{
						Console.WriteLine("Warning: New Actor referenced existing Actor as type?");
					}

					actorUpdate.TypeName = replay.Objects[actorUpdate.TypeId.TargetIndex];
					actorUpdate.ClassNetCache = TypeNameToClassNetCache(actorUpdate.TypeName, replay);
					actorUpdate.ObjectId = actorUpdate.ClassNetCache.ObjectIndex;
					actorUpdate.ObjectName = replay.Objects[actorUpdate.ObjectId];
					var typeName = $"RocketRP.Actors.{actorUpdate.ObjectName}";
					actorUpdate.Type = Type.GetType(typeName) ?? throw new Exception($"The Type {typeName} doesn't exist");

					if (actorUpdate.Type == null)
					{
						throw new Exception($"Unknown actor type: {actorUpdate.ObjectName}");
					}

					actorUpdate.Actor = (Actor)(Activator.CreateInstance(actorUpdate.Type) ?? throw new MissingMethodException($"{actorUpdate.TypeName} does not have a parameterless constructor"));
					actorUpdate.Actor.Deserialize(br, replay);
				}
				else
				{
					actorUpdate.State = ChannelState.Update;

					var activeActor = openChannels[actorUpdate.ChannelId];
					actorUpdate.NameId = activeActor.NameId;
					actorUpdate.Name = activeActor.Name;
					actorUpdate.TypeId = activeActor.TypeId;
					actorUpdate.TypeName = activeActor.TypeName;
					actorUpdate.ClassNetCache = activeActor.ClassNetCache;
					actorUpdate.ObjectId = activeActor.ObjectId;
					actorUpdate.ObjectName = activeActor.ObjectName;
					actorUpdate.Type = activeActor.Type;
					actorUpdate.Actor = (Actor)(Activator.CreateInstance(actorUpdate.Type) ?? throw new MissingMethodException($"{actorUpdate.TypeName} does not have a parameterless constructor"));
					//actorUpdate.SetPropertyObjectIndexes = new HashSet<int>();
					//actorUpdate.SetPropertyNames = new HashSet<string>();

					while (br.ReadBit())
					{
						var propId = br.ReadInt32((uint)actorUpdate.ClassNetCache.NumProperties);
						var propObjectIndex = actorUpdate.ClassNetCache.GetPropertyObjectIndex(propId);
						//actorUpdate.SetPropertyObjectIndexes.Add(propObjectIndex);
						//actorUpdate.SetPropertyNames.Add(propName);

						actorUpdate.Actor.DeserializeProperty(br, replay, propObjectIndex);
					}
				}
			}
			else
			{
				actorUpdate.State = ChannelState.Close;

				var activeActor = openChannels[actorUpdate.ChannelId];
				actorUpdate.NameId = activeActor.NameId;
				actorUpdate.Name = activeActor.Name;
				actorUpdate.TypeId = activeActor.TypeId;
				actorUpdate.TypeName = activeActor.TypeName;
				actorUpdate.ClassNetCache = activeActor.ClassNetCache;
				actorUpdate.ObjectId = activeActor.ObjectId;
				actorUpdate.ObjectName = activeActor.ObjectName;
				actorUpdate.Type = activeActor.Type;
			}

			return actorUpdate;
		}

		public void Serialize(BitWriter bw, Replay replay, Dictionary<int, ActorUpdate> openChannels)
		{
			bw.Write(ChannelId, (uint)replay.MaxChannels);

			if (State == ChannelState.Open)
			{
				bw.Write(true);
				bw.Write(true);

				if ((replay.EngineVersion >= 868 && replay.LicenseeVersion >= 15) ||
					(replay.EngineVersion == 868 && replay.LicenseeVersion == 14 && replay.Properties.MatchType != "Lan"))  // Fixes RLCS 2 replays
				{
					bw.Write(NameId ?? 0);
				}

				TypeId.Serialize(bw);
				Actor.Serialize(bw, replay);
				return;
			}
			else if (State == ChannelState.Update)
			{
				bw.Write(true);
				bw.Write(false);

				// We need to calulate the property object indexes if they haven't been set yet, and we need to get the ClassNetCache if it hasn't been set yet
				if (Actor.SetPropertyObjectIndexes.Count != Actor.SetPropertyNames.Count) Actor.CalculatePropertyObjectIndexes(replay);
				if (ClassNetCache == null) ClassNetCache = TypeNameToClassNetCache(TypeName, replay);

				foreach (var propObjectIndex in Actor.SetPropertyObjectIndexes)
				{
					var propId = ClassNetCache.GetPropertyPropertyId(propObjectIndex);
					var maxPropId = (uint)ClassNetCache.NumProperties;
					bw.Write(true);
					bw.Write(propId, maxPropId);
					Actor.SerializeProperty(bw, replay, propObjectIndex, propId, maxPropId);
				}
				bw.Write(false);
				return;
			}
			else if (State == ChannelState.Close)
			{
				bw.Write(false);
				return;
			}

			throw new Exception($"Unknown channel state: {State}");
		}

		public static ClassNetCache TypeNameToClassNetCache(string typeName, Replay replay)
		{
			switch (typeName)
			{
				case "ProjectX.Default__NetModeReplicator_X":
					return replay.ClassNetCacheByName["ProjectX.NetModeReplicator_X"];

				case "TAGame.Default__CameraSettingsActor_TA":
					return replay.ClassNetCacheByName["TAGame.CameraSettingsActor_TA"];
				case "TAGame.Default__MaxTimeWarningData_TA":
					return replay.ClassNetCacheByName["TAGame.MaxTimeWarningData_TA"];
				case "TAGame.Default__RumblePickups_TA":
					return replay.ClassNetCacheByName["TAGame.RumblePickups_TA"];
				case "TAGame.Default__PickupTimer_TA":
					return replay.ClassNetCacheByName["TAGame.PickupTimer_TA"];
				case "TAGame.Default__TrackerWallDynamicMeshActor_TA":
					return replay.ClassNetCacheByName["TAGame.TrackerWallDynamicMeshActor_TA"];
				case "TAGame.Default__FreeplayCommands_TA":
					return replay.ClassNetCacheByName["TAGame.FreeplayCommands_TA"];
				case "TAGame.Default__VoteActor_TA":
					return replay.ClassNetCacheByName["TAGame.VoteActor_TA"];

				case "TAGame.Default__PRI_TA":
					return replay.ClassNetCacheByName["TAGame.PRI_TA"];
				case "TAGame.Default__PRI_Breakout_TA":
					return replay.ClassNetCacheByName["TAGame.PRI_Breakout_TA"];
				case "TAGame.Default__PRI_KnockOut_TA":
					return replay.ClassNetCacheByName["TAGame.PRI_KnockOut_TA"];

				case "TAGame.Default__Car_TA":
				case "Archetypes.Car.Car_Default":
					return replay.ClassNetCacheByName["TAGame.Car_TA"];
				case "Archetypes.KnockOut.GameEvent_Knockout:CarArchetype":
					return replay.ClassNetCacheByName["TAGame.Car_KnockOut_TA"];
				case "Archetypes.Car.Car_PostGameLobby":
				case "Mutators.Mutators.Mutators.FreePlay:CarArchetype":
				case "Mutators.Mutators.Mutators.OnlineFreeplay:CarArchetype":
					return replay.ClassNetCacheByName["TAGame.Car_Freeplay_TA"];
				case "Archetypes.GameEvent.GameEvent_Season:CarArchetype":
					return replay.ClassNetCacheByName["TAGame.Car_Season_TA"];

				case "Archetypes.Ball.Ball_Default":
				case "Archetypes.Ball.Ball_Basketball":
				case "Archetypes.Ball.Ball_BasketBall":
				case "Archetypes.Ball.Ball_BasketBall_Mutator":
				case "Archetypes.Ball.Ball_Puck":
				case "Archetypes.Ball.CubeBall":
				case "Archetypes.Ball.Ball_Beachball":
				case "Archetypes.Ball.Ball_Anniversary":
				case "Archetypes.Ball.Ball_Football":
				case "Archetypes.Ball.Ball_Ekin":
				case "Archetypes.Ball.Ball_PizzaPuck":
				case "Archetypes.Ball.Ball_Shoe":
					return replay.ClassNetCacheByName["TAGame.Ball_TA"];
				case "Archetypes.Ball.Ball_Breakout":
				case "Archetypes.Ball.Ball_Score":
					return replay.ClassNetCacheByName["TAGame.Ball_Breakout_TA"];
				case "Archetypes.Ball.Ball_Haunted":
					return replay.ClassNetCacheByName["TAGame.Ball_Haunted_TA"];
				case "Archetypes.Ball.Ball_God":
					return replay.ClassNetCacheByName["TAGame.Ball_God_TA"];
				case "Archetypes.Ball.Ball_Fire":
					return replay.ClassNetCacheByName["TAGame.Ball_Fire_TA"];
				case "Archetypes.Ball.Ball_Training":
				case "Archetypes.Ball.Ball_Tutorial":
					return replay.ClassNetCacheByName["TAGame.Ball_Tutorial_TA"];
				case "Archetypes.Ball.Ball_Trajectory":
					return replay.ClassNetCacheByName["TAGame.Ball_Trajectory_TA"];

				case "Archetypes.CarComponents.CarComponent_Boost":
					return replay.ClassNetCacheByName["TAGame.CarComponent_Boost_TA"];
				case "Archetypes.CarComponents.CarComponent_Dodge":
					return replay.ClassNetCacheByName["TAGame.CarComponent_Dodge_TA"];
				case "Archetypes.CarComponents.CarComponent_DoubleJump":
					return replay.ClassNetCacheByName["TAGame.CarComponent_DoubleJump_TA"];
				case "Archetypes.CarComponents.CarComponent_FlipCar":
				case "Archetypes.KnockOut.GameEvent_Knockout:CarArchetype.Flip":
					return replay.ClassNetCacheByName["TAGame.CarComponent_FlipCar_TA"];
				case "Archetypes.CarComponents.CarComponent_Jump":
				case "Archetypes.KnockOut.GameEvent_Knockout:CarArchetype.Jump":
					return replay.ClassNetCacheByName["TAGame.CarComponent_Jump_TA"];
				case "Archetypes.Mutators.Mutator_Robin:AutoFlip":
					return replay.ClassNetCacheByName["TAGame.CarComponent_FlipCar_TA"];
				case "Archetypes.Mutators.Mutator_Robin:DoubleJump":
					return replay.ClassNetCacheByName["TAGame.CarComponent_DoubleJump_Robin_TA"];
				case "Archetypes.Mutators.Mutator_Robin:Jump":
					return replay.ClassNetCacheByName["TAGame.CarComponent_Jump_Robin_TA"];
				case "Archetypes.KnockOut.GameEvent_Knockout:CarArchetype.Boost":
					return replay.ClassNetCacheByName["TAGame.CarComponent_Boost_KO_TA"];
				case "Archetypes.KnockOut.GameEvent_Knockout:CarArchetype.Dodge":
					return replay.ClassNetCacheByName["TAGame.CarComponent_Dodge_KO_TA"];
				case "Archetypes.KnockOut.GameEvent_Knockout:CarArchetype.DoubleJump":
					return replay.ClassNetCacheByName["TAGame.CarComponent_DoubleJump_KO_TA"];
				case "Archetypes.KnockOut.GameEvent_Knockout:CarArchetype.StunlockArchetype":
					return replay.ClassNetCacheByName["TAGame.Stunlock_TA"];
				case "Archetypes.KnockOut.GameEvent_Knockout:CarArchetype.Torque":
					return replay.ClassNetCacheByName["TAGame.CarComponent_Torque_TA"];
				case "Archetypes.CarComponents.CarComponent_TerritoryDemolish":
					return replay.ClassNetCacheByName["TAGame.CarComponent_TerritoryDemolish_TA"];

				case "Archetypes.Teams.Team0":
				case "Archetypes.Teams.Team1":
					return replay.ClassNetCacheByName["TAGame.Team_Soccar_TA"];
				case "Archetypes.Teams.TeamWhite0":
				case "Archetypes.Teams.TeamWhite1":
					return replay.ClassNetCacheByName["TAGame.Team_Freeplay_TA"];


				case "Archetypes.GameEvent.GameEvent_Basketball":
				case "Archetypes.GameEvent.GameEvent_Hockey":
				case "Archetypes.GameEvent.GameEvent_Soccar":
				case "Archetypes.GameEvent.GameEvent_Items":
				case "Archetypes.GameEvent.GameEvent_SoccarLan":
				case "GameInfo_Basketball.GameInfo.GameInfo_Basketball:Archetype":
				case "Gameinfo_Hockey.GameInfo.Gameinfo_Hockey:Archetype":
				case "GameInfo_BumperCars.GameInfo.GameInfo_BumperCars:Archetype":
				case "GameInfo_DemoDerby.GameInfo.GameInfo_DemoDerby:Archetype":
				case "GameInfo_GoalCrazy.GameInfo.GameInfo_GoalCrazy:Archetype":
				case "GameInfo_Hops.GameInfo.GameInfo_Hops:Archetype":
				case "GameInfo_Possession.GameInfo.GameInfo_Possession:Archetype":
				case "GameInfo_TargetAcquired.GameInfo.GameInfo_TargetAcquired:Archetype":
				case "GameInfo_LTM_AprilFool.GameInfo.GameInfo_LTM_AprilFool:Archetype":
				case "GameInfo_LTM_BeachBall.GameInfo.GameInfo_LTM_BeachBall:Archetype":
				case "GameInfo_LTM_BoomerBall.GameInfo.GameInfo_LTM_BoomerBall:Archetype":
				case "GameInfo_LTM_Demolition.GameInfo.GameInfo_LTM_Demolition:Archetype":
				case "GameInfo_LTM_Eggstra.GameInfo.GameInfo_LTM_Eggstra:Archetype":
				case "GameInfo_LTM_GForce.GameInfo.GameInfo_LTM_GForce:Archetype":
				case "GameInfo_LTM_Moonball.GameInfo.GameInfo_LTM_Moonball:Archetype":
				case "GameInfo_LTM_Pinball.GameInfo.GameInfo_LTM_Pinball:Archetype":
				case "GameInfo_LTM_SpeedDemon.GameInfo.GameInfo_LTM_SpeedDemon:Archetype":
				case "GameInfo_LTM_SpikeRush.GameInfo.GameInfo_LTM_SpikeRush:Archetype":
				case "GameInfo_LTM_SuperCube.GameInfo.GameInfo_LTM_SuperCube:Archetype":
					return replay.ClassNetCacheByName["TAGame.GameEvent_Soccar_TA"];
				case "Archetypes.GameEvent.GameEvent_SoccarPrivate":
				case "Archetypes.GameEvent.GameEvent_BasketballPrivate":
				case "Archetypes.GameEvent.GameEvent_HockeyPrivate":
					return replay.ClassNetCacheByName["TAGame.GameEvent_SoccarPrivate_TA"];
				case "Archetypes.GameEvent.GameEvent_SoccarSplitscreen":
				case "Archetypes.GameEvent.GameEvent_BasketballSplitscreen":
				case "Archetypes.GameEvent.GameEvent_HockeySplitscreen":
					return replay.ClassNetCacheByName["TAGame.GameEvent_SoccarSplitscreen_TA"];
				case "Archetypes.GameEvent.GameEvent_Season":
					return replay.ClassNetCacheByName["TAGame.GameEvent_Season_TA"];
				case "Archetypes.GameEvent.GameEvent_Breakout":
				case "GameInfo_LTM_DropshotRumble.GameInfo.GameInfo_LTM_DropshotRumble:Archetype":
					return replay.ClassNetCacheByName["TAGame.GameEvent_Breakout_TA"];
				case "Archetypes.GameEvent.GameEvent_FTE_Part1_Prime":
					return replay.ClassNetCacheByName["TAGame.GameEvent_FTE_TA"];
				case "Archetypes.KnockOut.GameEvent_Knockout":
					return replay.ClassNetCacheByName["TAGame.GameEvent_KnockOut_TA"];
				case "gameinfo_godball.GameInfo.gameinfo_godball:Archetype":
				case "GameInfo_GodBall.GameInfo.GameInfo_GodBall:Archetype":
				case "GameInfo_HeatseekerTerritory.GameInfo.GameInfo_HeatseekerTerritory:Archetype":
				case "GameInfo_MultiHeatseeker.GameInfo.GameInfo_MultiHeatseeker:Archetype":
					return replay.ClassNetCacheByName["TAGame.GameEvent_GodBall_TA"];
				case "GameInfo_FootBall.GameInfo.GameInfo_FootBall:Archetype":
					return replay.ClassNetCacheByName["TAGame.GameEvent_Football_TA"];
				case "GameInfo_Territory.GameInfo.GameInfo_Territory:Archetype":
				case "GameInfo_SnowDayTerritory.GameInfo.GameInfo_SnowDayTerritory:Archetype":
					return replay.ClassNetCacheByName["TAGame.GameEvent_Territory_TA"];
				/* These events shouldn't ever occur but there was at least 1 replay that had "GameInfo_Tutorial.GameEvent.GameEvent_Tutorial_Aerial" */
				case "Archetypes.GameEvent.GameEvent_Tutorial_Advanced":
					return replay.ClassNetCacheByName["TAGame.GameEvent_Tutorial_Advanced_TA"];
				case "Archetypes.GameEvent.GameEvent_Tutorial_Basic":
					return replay.ClassNetCacheByName["TAGame.GameEvent_Tutorial_Basic_TA"];
				case "Archetypes.GameEvent.GameEvent_Tutorial_FreePlay":
					return replay.ClassNetCacheByName["TAGame.GameEvent_Tutorial_FreePlay_TA"];
				case "GameInfo_Tutorial.GameEvent.GameEvent_Tutorial_Aerial":
					return replay.ClassNetCacheByName["TAGame.GameEvent_Training_Aerial_TA"];
				case "GameInfo_Tutorial.GameEvent.GameEvent_Tutorial_Goalie":
					return replay.ClassNetCacheByName["TAGame.GameEvent_Training_Goalie_TA"];
				case "GameInfo_Tutorial.GameEvent.GameEvent_Tutorial_Striker":
					return replay.ClassNetCacheByName["TAGame.GameEvent_Training_Striker_TA"];

				case "Archetypes.SpecialPickups.SpecialPickup_GravityWell":
				case "Archetypes.SpecialPickups.BM.SpecialPickup_GravityWell_BM":
					return replay.ClassNetCacheByName["TAGame.SpecialPickup_BallGravity_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_BallVelcro":
				case "Archetypes.SpecialPickups.BM.SpecialPickup_BallVelcro_BM":
					return replay.ClassNetCacheByName["TAGame.SpecialPickup_BallVelcro_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_BallLasso":
				case "Archetypes.SpecialPickups.BM.SpecialPickup_BallLasso_BM":
					return replay.ClassNetCacheByName["TAGame.SpecialPickup_BallLasso_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_BallGrapplingHook":
				case "Archetypes.SpecialPickups.BM.SpecialPickup_BallGrapplingHook_BM":
					return replay.ClassNetCacheByName["TAGame.SpecialPickup_GrapplingHook_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_Swapper":
				case "Archetypes.SpecialPickups.BM.SpecialPickup_Swapper_BM":
					return replay.ClassNetCacheByName["TAGame.SpecialPickup_Swapper_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_BallFreeze":
				case "Archetypes.SpecialPickups.BM.SpecialPickup_BallFreeze_BM":
				case "Archetypes.Mutators.SubRules.ItemsMode_RPS:DispenserArchetype.ItemPool.Obj_2":
					return replay.ClassNetCacheByName["TAGame.SpecialPickup_BallFreeze_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_BoostOverride":
				case "Archetypes.SpecialPickups.BM.SpecialPickup_BoostOverride_BM":
					return replay.ClassNetCacheByName["TAGame.SpecialPickup_BoostOverride_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_Tornado":
				case "Archetypes.SpecialPickups.BM.SpecialPickup_Tornado_BM":
					return replay.ClassNetCacheByName["TAGame.SpecialPickup_Tornado_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_BallSpring":
				case "Archetypes.SpecialPickups.SpecialPickup_CarSpring":
				case "Archetypes.SpecialPickups.BM.SpecialPickup_BallSpring_BM":
				case "Archetypes.SpecialPickups.BM.SpecialPickup_CarSpring_BM":
				case "Archetypes.Mutators.SubRules.ItemsMode_RPS:DispenserArchetype.ItemPool.Obj":
				case "Archetypes.Mutators.SubRules.ItemsMode_RPS:DispenserArchetype.ItemPool.Obj_1":
					return replay.ClassNetCacheByName["TAGame.SpecialPickup_BallCarSpring_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_StrongHit":
				case "Archetypes.SpecialPickups.BM.SpecialPickup_StrongHit_BM":
					return replay.ClassNetCacheByName["TAGame.SpecialPickup_HitForce_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_Batarang":
					return replay.ClassNetCacheByName["TAGame.SpecialPickup_Batarang_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_HauntedBallBeam":
					return replay.ClassNetCacheByName["TAGame.SpecialPickup_HauntedBallBeam_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_Rugby":
				case "Archetypes.SpecialPickups.SpecialPickup_RugbyLightDark":
					return replay.ClassNetCacheByName["TAGame.SpecialPickup_Rugby_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_Football":
					return replay.ClassNetCacheByName["TAGame.SpecialPickup_Football_TA"];

				case "GameInfo_Basketball.GameInfo.GameInfo_Basketball:GameReplicationInfoArchetype":
				case "Gameinfo_Hockey.GameInfo.Gameinfo_Hockey:GameReplicationInfoArchetype":
				case "GameInfo_Season.GameInfo.GameInfo_Season:GameReplicationInfoArchetype":
				case "GameInfo_Soccar.GameInfo.GameInfo_Soccar:GameReplicationInfoArchetype":
				case "GameInfo_Items.GameInfo.GameInfo_Items:GameReplicationInfoArchetype":
				case "GameInfo_Breakout.GameInfo.GameInfo_Breakout:GameReplicationInfoArchetype":
				case "gameinfo_godball.GameInfo.gameinfo_godball:GameReplicationInfoArchetype":
				case "GameInfo_GodBall.GameInfo.GameInfo_GodBall:GameReplicationInfoArchetype":
				case "GameInfo_FootBall.GameInfo.GameInfo_FootBall:GameReplicationInfoArchetype":
				case "GameInfo_FTE.GameInfo.GameInfo_FTE:GameReplicationInfoArchetype":
				case "GameInfo_KnockOut.KnockOut.GameInfo_KnockOut:GameReplicationInfoArchetype":
				case "GameInfo_Tutorial.GameInfo.GameInfo_Tutorial:GameReplicationInfoArchetype":
				case "GameInfo_Territory.GameInfo.GameInfo_Territory:GameReplicationInfoArchetype":
				case "GameInfo_HeatseekerTerritory.GameInfo.GameInfo_HeatseekerTerritory:GameReplicationInfoArchetype":
				case "GameInfo_SnowDayTerritory.GameInfo.GameInfo_SnowDayTerritory:GameReplicationInfoArchetype":
				case "GameInfo_BumperCars.GameInfo.GameInfo_BumperCars:GameReplicationInfoArchetype":
				case "GameInfo_DemoDerby.GameInfo.GameInfo_DemoDerby:GameReplicationInfoArchetype":
				case "GameInfo_GoalCrazy.GameInfo.GameInfo_GoalCrazy:GameReplicationInfoArchetype":
				case "GameInfo_Hops.GameInfo.GameInfo_Hops:GameReplicationInfoArchetype":
				case "GameInfo_MultiHeatseeker.GameInfo.GameInfo_MultiHeatseeker:GameReplicationInfoArchetype":
				case "GameInfo_Possession.GameInfo.GameInfo_Possession:GameReplicationInfoArchetype":
				case "GameInfo_TargetAcquired.GameInfo.GameInfo_TargetAcquired:GameReplicationInfoArchetype":
				case "GameInfo_LTM_AprilFool.GameInfo.GameInfo_LTM_AprilFool:GameReplicationInfoArchetype":
				case "GameInfo_LTM_BeachBall.GameInfo.GameInfo_LTM_BeachBall:GameReplicationInfoArchetype":
				case "GameInfo_LTM_BoomerBall.GameInfo.GameInfo_LTM_BoomerBall:GameReplicationInfoArchetype":
				case "GameInfo_LTM_Demolition.GameInfo.GameInfo_LTM_Demolition:GameReplicationInfoArchetype":
				case "GameInfo_LTM_DropshotRumble.GameInfo.GameInfo_LTM_DropshotRumble:GameReplicationInfoArchetype":
				case "GameInfo_LTM_Eggstra.GameInfo.GameInfo_LTM_Eggstra:GameReplicationInfoArchetype":
				case "GameInfo_LTM_GForce.GameInfo.GameInfo_LTM_GForce:GameReplicationInfoArchetype":
				case "GameInfo_LTM_Moonball.GameInfo.GameInfo_LTM_Moonball:GameReplicationInfoArchetype":
				case "GameInfo_LTM_Pinball.GameInfo.GameInfo_LTM_Pinball:GameReplicationInfoArchetype":
				case "GameInfo_LTM_SpeedDemon.GameInfo.GameInfo_LTM_SpeedDemon:GameReplicationInfoArchetype":
				case "GameInfo_LTM_SpikeRush.GameInfo.GameInfo_LTM_SpikeRush:GameReplicationInfoArchetype":
				case "GameInfo_LTM_SuperCube.GameInfo.GameInfo_LTM_SuperCube:GameReplicationInfoArchetype":
					return replay.ClassNetCacheByName["TAGame.GRI_TA"];

				case "Archetypes.Tutorial.Cannon":
					return replay.ClassNetCacheByName["TAGame.Cannon_TA"];
			}

			// These are map specific objects
			if (typeName.Contains("CrowdActor_TA"))
			{
				return replay.ClassNetCacheByName["TAGame.CrowdActor_TA"];
			}
			else if (typeName.Contains("VehiclePickup_Boost_TA"))
			{
				return replay.ClassNetCacheByName["TAGame.VehiclePickup_Boost_TA"];
			}
			else if (typeName.Contains("CrowdManager_TA"))
			{
				return replay.ClassNetCacheByName["TAGame.CrowdManager_TA"];
			}
			else if (typeName.Contains("BreakOutActor_Platform_TA"))
			{
				return replay.ClassNetCacheByName["TAGame.BreakOutActor_Platform_TA"];
			}
			else if (typeName.Contains("PlayerStart_Platform_TA"))
			{
				return replay.ClassNetCacheByName["TAGame.PlayerStart_Platform_TA"];
			}
			else if (typeName.Contains("InMapScoreboard_TA"))
			{
				return replay.ClassNetCacheByName["TAGame.InMapScoreboard_TA"];
			}
			else if (typeName.Contains("HauntedBallTrapTrigger_TA"))
			{
				return replay.ClassNetCacheByName["TAGame.HauntedBallTrapTrigger_TA"];
			}

			throw new Exception($"Unknown type name: {typeName}");
		}
	}

	public enum ChannelState : byte
	{
		Close,
		Update,
		Open,
	}
}
