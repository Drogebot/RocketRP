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
		public ObjectTarget TypeId { get; set; }
		public string TypeName { get; set; }
		public int ObjectId { get; set; }
		public string ObjectName { get; set; }
		public Type? Type { get; set; }
		public Actor Actor { get; set; }

		private ClassNetCache ClassNetCache;
		public HashSet<int> SetPropertyObjectIndexes;
		public HashSet<string> SetPropertyNames;

		public static ActorUpdate Deserialize(BitReader br, Replay replay, Dictionary<int, ActorUpdate> openChannels)
		{
			var actorUpdate = new ActorUpdate();

			actorUpdate.ChannelId = br.ReadInt32Max(replay.MaxChannels);

			if (br.ReadBit())
			{
				if (br.ReadBit())
				{
					actorUpdate.State = ChannelState.Open;

					if ((replay.EngineVersion >= 868 && replay.LicenseeVersion >= 15) ||
						(replay.EngineVersion == 868 && replay.LicenseeVersion == 14 && replay.Properties.MatchType != "Lan"))	// Fixes RLCS 2 replays
					{
						actorUpdate.NameId = br.ReadInt32();
						actorUpdate.Name = replay.Names[actorUpdate.NameId.Value];
					}

					actorUpdate.TypeId = ObjectTarget.Deserialize(br);
					if (actorUpdate.TypeId.IsActor)
					{
						Console.WriteLine("Warning: New Actor referenced existing Actor as type?");
					}

					actorUpdate.TypeName = replay.Objects[actorUpdate.TypeId.TargetIndex];
					actorUpdate.ClassNetCache = TypeNameToClassNetCache(actorUpdate.TypeName, replay);
					actorUpdate.ObjectId = actorUpdate.ClassNetCache.ObjectIndex;
					actorUpdate.ObjectName = replay.Objects[actorUpdate.ObjectId];
					actorUpdate.Type = System.Type.GetType($"RocketRP.Actors.{actorUpdate.ObjectName}");

					if(actorUpdate.Type == null)
					{
						throw new Exception($"Unknown actor type: {actorUpdate.ObjectName}");
					}

					actorUpdate.Actor = (Actor)Activator.CreateInstance(actorUpdate.Type);
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
					actorUpdate.Actor = (Actor)Activator.CreateInstance(actorUpdate.Type);
					actorUpdate.SetPropertyObjectIndexes = new HashSet<int>();
					actorUpdate.SetPropertyNames = new HashSet<string>();

					while (br.ReadBit())
					{
						var propId = br.ReadInt32Max(actorUpdate.ClassNetCache.NumProperties);
						var propObjectIndex = actorUpdate.ClassNetCache.GetPropertyObjectIndex(propId);
						actorUpdate.SetPropertyObjectIndexes.Add(propObjectIndex);
						actorUpdate.SetPropertyNames.Add(replay.Objects[propObjectIndex]);

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
			bw.Write(ChannelId, replay.MaxChannels);

			if (State == ChannelState.Open)
			{
				bw.Write(true);
				bw.Write(true);

				if ((replay.EngineVersion >= 868 && replay.LicenseeVersion >= 15) ||
					(replay.EngineVersion == 868 && replay.LicenseeVersion == 14 && replay.Properties.MatchType != "Lan"))	// Fixes RLCS 2 replays
				{
					bw.Write(NameId.Value);
				}

				TypeId.Serialize(bw);
				Actor.Serialize(bw, replay);
				return;
			}
			else if(State == ChannelState.Update)
			{
				bw.Write(true);
				bw.Write(false);

				// We need to calulate the property object indexes if they haven't been set yet, and we need to get the ClassNetCache if it hasn't been set yet
				if (Actor.SetPropertyObjectIndexes.Count != Actor.SetPropertyNames.Count) Actor.CalculatePropertyObjectIndexes(replay);
				if(ClassNetCache == null) ClassNetCache = TypeNameToClassNetCache(TypeName, replay);

				foreach (var propObjectIndex in Actor.SetPropertyObjectIndexes)
				{
					bw.Write(true);
					var propId = ClassNetCache.GetPropertyPropertyId(propObjectIndex);
					bw.Write(propId, ClassNetCache.NumProperties);
					Actor.SerializeProperty(bw, replay, propId, propObjectIndex);
				}
				bw.Write(false);
				return;
			}
			else if(State == ChannelState.Close)
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
					return replay.ClassNetCacheByName["TAGame.Ball_TA"];
				case "Archetypes.Ball.Ball_Breakout":
					return replay.ClassNetCacheByName["TAGame.Ball_Breakout_TA"];
				case "Archetypes.Ball.Ball_Haunted":
					return replay.ClassNetCacheByName["TAGame.Ball_Haunted_TA"];
				case "Archetypes.Ball.Ball_God":
					return replay.ClassNetCacheByName["TAGame.Ball_God_TA"];
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
					return replay.ClassNetCacheByName["TAGame.GameEvent_Breakout_TA"];
				case "Archetypes.GameEvent.GameEvent_FTE_Part1_Prime":
					return replay.ClassNetCacheByName["TAGame.GameEvent_FTE_TA"];
				case "Archetypes.KnockOut.GameEvent_Knockout":
					return replay.ClassNetCacheByName["TAGame.GameEvent_KnockOut_TA"];
				case "gameinfo_godball.GameInfo.gameinfo_godball:Archetype":
				case "GameInfo_GodBall.GameInfo.GameInfo_GodBall:Archetype":
					return replay.ClassNetCacheByName["TAGame.GameEvent_GodBall_TA"];
				case "GameInfo_FootBall.GameInfo.GameInfo_FootBall:Archetype":
					return replay.ClassNetCacheByName["TAGame.GameEvent_Football_TA"];
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
					return replay.ClassNetCacheByName["TAGame.SpecialPickup_BallGravity_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_BallVelcro":
					return replay.ClassNetCacheByName["TAGame.SpecialPickup_BallVelcro_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_BallLasso":
					return replay.ClassNetCacheByName["TAGame.SpecialPickup_BallLasso_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_BallGrapplingHook":
					return replay.ClassNetCacheByName["TAGame.SpecialPickup_GrapplingHook_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_Swapper":
					return replay.ClassNetCacheByName["TAGame.SpecialPickup_Swapper_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_BallFreeze":
					return replay.ClassNetCacheByName["TAGame.SpecialPickup_BallFreeze_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_BoostOverride":
					return replay.ClassNetCacheByName["TAGame.SpecialPickup_BoostOverride_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_Tornado":
					return replay.ClassNetCacheByName["TAGame.SpecialPickup_Tornado_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_BallSpring":
				case "Archetypes.SpecialPickups.SpecialPickup_CarSpring":
					return replay.ClassNetCacheByName["TAGame.SpecialPickup_BallCarSpring_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_StrongHit":
					return replay.ClassNetCacheByName["TAGame.SpecialPickup_HitForce_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_Batarang":
					return replay.ClassNetCacheByName["TAGame.SpecialPickup_Batarang_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_HauntedBallBeam":
					return replay.ClassNetCacheByName["TAGame.SpecialPickup_HauntedBallBeam_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_Rugby":
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
