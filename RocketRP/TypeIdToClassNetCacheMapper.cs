using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
	public class TypeIdToClassNetCacheMapper
	{
		public static Dictionary<int, ClassNetCache> MapTypeIdsToClassNetCache(List<string> objects, List<ClassNetCache> classNetCaches)
		{
			var typeIdToClassNetCache = new Dictionary<int, ClassNetCache>();
			var classNetCacheByName = classNetCaches.ToDictionary(c => objects[c.ObjectIndex], c => c);
			for (int i = 0; i < objects.Count; i++)
			{
				string typeName = objects[i];
				var classNetCache = TypeNameToClassNetCache(typeName, classNetCacheByName);
				if (classNetCache is not null)
				{
					//Console.WriteLine($"Mapping TypeId {i} ({typeName}) to ClassNetCache of {classNetCache.ClassType.Name} ({classNetCache.ObjectIndex})");
					typeIdToClassNetCache.Add(i, classNetCache);
				}
			}
			return typeIdToClassNetCache;
		}

		public static ClassNetCache? TypeNameToClassNetCache(string typeName, Dictionary<string, ClassNetCache> classNetCacheByName)
		{
			switch (typeName)
			{
				case "ProjectX.Default__NetModeReplicator_X":
					return classNetCacheByName["ProjectX.NetModeReplicator_X"];

				case "TAGame.Default__CameraSettingsActor_TA":
					return classNetCacheByName["TAGame.CameraSettingsActor_TA"];
				case "TAGame.Default__MaxTimeWarningData_TA":
					return classNetCacheByName["TAGame.MaxTimeWarningData_TA"];
				case "TAGame.Default__RumblePickups_TA":
					return classNetCacheByName["TAGame.RumblePickups_TA"];
				case "TAGame.Default__PickupTimer_TA":
					return classNetCacheByName["TAGame.PickupTimer_TA"];
				case "TAGame.Default__TrackerWallDynamicMeshActor_TA":
					return classNetCacheByName["TAGame.TrackerWallDynamicMeshActor_TA"];
				case "TAGame.Default__FreeplayCommands_TA":
					return classNetCacheByName["TAGame.FreeplayCommands_TA"];
				case "TAGame.Default__VoteActor_TA":
					return classNetCacheByName["TAGame.VoteActor_TA"];

				case "TAGame.Default__PRI_TA":
					return classNetCacheByName["TAGame.PRI_TA"];
				case "TAGame.Default__PRI_Breakout_TA":
					return classNetCacheByName["TAGame.PRI_Breakout_TA"];
				case "TAGame.Default__PRI_KnockOut_TA":
					return classNetCacheByName["TAGame.PRI_KnockOut_TA"];
				case "TAGame.Default__PRI_Possession_TA":
					return classNetCacheByName["TAGame.PRI_Possession_TA"];

				case "TAGame.Default__Car_TA":
				case "Archetypes.Car.Car_Default":
					return classNetCacheByName["TAGame.Car_TA"];
				case "Archetypes.KnockOut.GameEvent_Knockout:CarArchetype":
					return classNetCacheByName["TAGame.Car_KnockOut_TA"];
				case "Archetypes.Car.Car_PostGameLobby":
				case "Mutators.Mutators.Mutators.FreePlay:CarArchetype":
				case "Mutators.Mutators.Mutators.OnlineFreeplay:CarArchetype":
					return classNetCacheByName["TAGame.Car_Freeplay_TA"];
				case "Archetypes.GameEvent.GameEvent_Season:CarArchetype":
					return classNetCacheByName["TAGame.Car_Season_TA"];

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
					return classNetCacheByName["TAGame.Ball_TA"];
				case "Archetypes.Ball.Ball_Breakout":
				case "Archetypes.Ball.Ball_Score":
					return classNetCacheByName["TAGame.Ball_Breakout_TA"];
				case "Archetypes.Ball.Ball_Haunted":
					return classNetCacheByName["TAGame.Ball_Haunted_TA"];
				case "Archetypes.Ball.Ball_God":
					return classNetCacheByName["TAGame.Ball_God_TA"];
				case "Archetypes.Ball.Ball_Fire":
					return classNetCacheByName["TAGame.Ball_Fire_TA"];
				case "Archetypes.Ball.Ball_Training":
				case "Archetypes.Ball.Ball_Tutorial":
					return classNetCacheByName["TAGame.Ball_Tutorial_TA"];
				case "Archetypes.Ball.Ball_Trajectory":
					return classNetCacheByName["TAGame.Ball_Trajectory_TA"];

				case "Archetypes.CarComponents.CarComponent_Boost":
					return classNetCacheByName["TAGame.CarComponent_Boost_TA"];
				case "Archetypes.CarComponents.CarComponent_Dodge":
					return classNetCacheByName["TAGame.CarComponent_Dodge_TA"];
				case "Archetypes.CarComponents.CarComponent_DoubleJump":
					return classNetCacheByName["TAGame.CarComponent_DoubleJump_TA"];
				case "Archetypes.CarComponents.CarComponent_FlipCar":
				case "Archetypes.KnockOut.GameEvent_Knockout:CarArchetype.Flip":
					return classNetCacheByName["TAGame.CarComponent_FlipCar_TA"];
				case "Archetypes.CarComponents.CarComponent_Jump":
				case "Archetypes.KnockOut.GameEvent_Knockout:CarArchetype.Jump":
					return classNetCacheByName["TAGame.CarComponent_Jump_TA"];
				case "Archetypes.Mutators.Mutator_Robin:AutoFlip":
					return classNetCacheByName["TAGame.CarComponent_FlipCar_TA"];
				case "Archetypes.Mutators.Mutator_Robin:DoubleJump":
					return classNetCacheByName["TAGame.CarComponent_DoubleJump_Robin_TA"];
				case "Archetypes.Mutators.Mutator_Robin:Jump":
					return classNetCacheByName["TAGame.CarComponent_Jump_Robin_TA"];
				case "Archetypes.KnockOut.GameEvent_Knockout:CarArchetype.Boost":
					return classNetCacheByName["TAGame.CarComponent_Boost_KO_TA"];
				case "Archetypes.KnockOut.GameEvent_Knockout:CarArchetype.Dodge":
					return classNetCacheByName["TAGame.CarComponent_Dodge_KO_TA"];
				case "Archetypes.KnockOut.GameEvent_Knockout:CarArchetype.DoubleJump":
					return classNetCacheByName["TAGame.CarComponent_DoubleJump_KO_TA"];
				case "Archetypes.KnockOut.GameEvent_Knockout:CarArchetype.StunlockArchetype":
					return classNetCacheByName["TAGame.Stunlock_TA"];
				case "Archetypes.KnockOut.GameEvent_Knockout:CarArchetype.Torque":
					return classNetCacheByName["TAGame.CarComponent_Torque_TA"];
				case "Archetypes.CarComponents.CarComponent_TerritoryDemolish":
					return classNetCacheByName["TAGame.CarComponent_TerritoryDemolish_TA"];

				case "Archetypes.Teams.Team0":
				case "Archetypes.Teams.Team1":
					return classNetCacheByName["TAGame.Team_Soccar_TA"];
				case "Archetypes.Teams.TeamWhite0":
				case "Archetypes.Teams.TeamWhite1":
					return classNetCacheByName["TAGame.Team_Freeplay_TA"];


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
					return classNetCacheByName["TAGame.GameEvent_Soccar_TA"];
				case "Archetypes.GameEvent.GameEvent_SoccarPrivate":
				case "Archetypes.GameEvent.GameEvent_BasketballPrivate":
				case "Archetypes.GameEvent.GameEvent_HockeyPrivate":
					return classNetCacheByName["TAGame.GameEvent_SoccarPrivate_TA"];
				case "Archetypes.GameEvent.GameEvent_SoccarSplitscreen":
				case "Archetypes.GameEvent.GameEvent_BasketballSplitscreen":
				case "Archetypes.GameEvent.GameEvent_HockeySplitscreen":
					return classNetCacheByName["TAGame.GameEvent_SoccarSplitscreen_TA"];
				case "Archetypes.GameEvent.GameEvent_Season":
					return classNetCacheByName["TAGame.GameEvent_Season_TA"];
				case "Archetypes.GameEvent.GameEvent_Breakout":
				case "GameInfo_LTM_DropshotRumble.GameInfo.GameInfo_LTM_DropshotRumble:Archetype":
					return classNetCacheByName["TAGame.GameEvent_Breakout_TA"];
				case "Archetypes.GameEvent.GameEvent_FTE_Part1_Prime":
					return classNetCacheByName["TAGame.GameEvent_FTE_TA"];
				case "Archetypes.KnockOut.GameEvent_Knockout":
					return classNetCacheByName["TAGame.GameEvent_KnockOut_TA"];
				case "gameinfo_godball.GameInfo.gameinfo_godball:Archetype":
				case "GameInfo_GodBall.GameInfo.GameInfo_GodBall:Archetype":
				case "GameInfo_HeatseekerTerritory.GameInfo.GameInfo_HeatseekerTerritory:Archetype":
				case "GameInfo_MultiHeatseeker.GameInfo.GameInfo_MultiHeatseeker:Archetype":
					return classNetCacheByName["TAGame.GameEvent_GodBall_TA"];
				case "GameInfo_FootBall.GameInfo.GameInfo_FootBall:Archetype":
					return classNetCacheByName["TAGame.GameEvent_Football_TA"];
				case "GameInfo_Territory.GameInfo.GameInfo_Territory:Archetype":
				case "GameInfo_SnowDayTerritory.GameInfo.GameInfo_SnowDayTerritory:Archetype":
					return classNetCacheByName["TAGame.GameEvent_Territory_TA"];
				/* These events shouldn't ever occur but there was at least 1 replay that had "GameInfo_Tutorial.GameEvent.GameEvent_Tutorial_Aerial" */
				case "Archetypes.GameEvent.GameEvent_Tutorial_Advanced":
					return classNetCacheByName["TAGame.GameEvent_Tutorial_Advanced_TA"];
				case "Archetypes.GameEvent.GameEvent_Tutorial_Basic":
					return classNetCacheByName["TAGame.GameEvent_Tutorial_Basic_TA"];
				case "Archetypes.GameEvent.GameEvent_Tutorial_FreePlay":
					return classNetCacheByName["TAGame.GameEvent_Tutorial_FreePlay_TA"];
				case "GameInfo_Tutorial.GameEvent.GameEvent_Tutorial_Aerial":
					return classNetCacheByName["TAGame.GameEvent_Training_Aerial_TA"];
				case "GameInfo_Tutorial.GameEvent.GameEvent_Tutorial_Goalie":
					return classNetCacheByName["TAGame.GameEvent_Training_Goalie_TA"];
				case "GameInfo_Tutorial.GameEvent.GameEvent_Tutorial_Striker":
					return classNetCacheByName["TAGame.GameEvent_Training_Striker_TA"];

				case "Archetypes.SpecialPickups.SpecialPickup_GravityWell":
				case "Archetypes.SpecialPickups.BM.SpecialPickup_GravityWell_BM":
					return classNetCacheByName["TAGame.SpecialPickup_BallGravity_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_BallVelcro":
				case "Archetypes.SpecialPickups.BM.SpecialPickup_BallVelcro_BM":
					return classNetCacheByName["TAGame.SpecialPickup_BallVelcro_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_BallLasso":
				case "Archetypes.SpecialPickups.BM.SpecialPickup_BallLasso_BM":
					return classNetCacheByName["TAGame.SpecialPickup_BallLasso_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_BallGrapplingHook":
				case "Archetypes.SpecialPickups.BM.SpecialPickup_BallGrapplingHook_BM":
					return classNetCacheByName["TAGame.SpecialPickup_GrapplingHook_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_Swapper":
				case "Archetypes.SpecialPickups.BM.SpecialPickup_Swapper_BM":
					return classNetCacheByName["TAGame.SpecialPickup_Swapper_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_BallFreeze":
				case "Archetypes.SpecialPickups.BM.SpecialPickup_BallFreeze_BM":
				case "Archetypes.Mutators.SubRules.ItemsMode_RPS:DispenserArchetype.ItemPool.Obj_2":
					return classNetCacheByName["TAGame.SpecialPickup_BallFreeze_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_BoostOverride":
				case "Archetypes.SpecialPickups.BM.SpecialPickup_BoostOverride_BM":
					return classNetCacheByName["TAGame.SpecialPickup_BoostOverride_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_Tornado":
				case "Archetypes.SpecialPickups.BM.SpecialPickup_Tornado_BM":
					return classNetCacheByName["TAGame.SpecialPickup_Tornado_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_BallSpring":
				case "Archetypes.SpecialPickups.SpecialPickup_CarSpring":
				case "Archetypes.SpecialPickups.BM.SpecialPickup_BallSpring_BM":
				case "Archetypes.SpecialPickups.BM.SpecialPickup_CarSpring_BM":
				case "Archetypes.Mutators.SubRules.ItemsMode_RPS:DispenserArchetype.ItemPool.Obj":
				case "Archetypes.Mutators.SubRules.ItemsMode_RPS:DispenserArchetype.ItemPool.Obj_1":
					return classNetCacheByName["TAGame.SpecialPickup_BallCarSpring_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_StrongHit":
				case "Archetypes.SpecialPickups.BM.SpecialPickup_StrongHit_BM":
					return classNetCacheByName["TAGame.SpecialPickup_HitForce_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_Batarang":
					return classNetCacheByName["TAGame.SpecialPickup_Batarang_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_HauntedBallBeam":
					return classNetCacheByName["TAGame.SpecialPickup_HauntedBallBeam_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_Rugby":
				case "Archetypes.SpecialPickups.SpecialPickup_RugbyLightDark":
					return classNetCacheByName["TAGame.SpecialPickup_Rugby_TA"];
				case "Archetypes.SpecialPickups.SpecialPickup_Football":
					return classNetCacheByName["TAGame.SpecialPickup_Football_TA"];

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
					return classNetCacheByName["TAGame.GRI_TA"];

				case "Archetypes.Tutorial.Cannon":
					return classNetCacheByName["TAGame.Cannon_TA"];
				default:
					// These are map specific objects, they should all contain ".TheWorld:PersistentLevel." before as well
					if (typeName.Contains("VehiclePickup_Boost_TA_"))
					{
						return classNetCacheByName["TAGame.VehiclePickup_Boost_TA"];
					}
					else if (typeName.Contains("CrowdActor_TA_"))
					{
						return classNetCacheByName["TAGame.CrowdActor_TA"];
					}
					else if (typeName.Contains("CrowdManager_TA_"))
					{
						return classNetCacheByName["TAGame.CrowdManager_TA"];
					}
					else if (typeName.Contains("BreakOutActor_Platform_TA_"))
					{
						return classNetCacheByName["TAGame.BreakOutActor_Platform_TA"];
					}
					else if (typeName.Contains("PlayerStart_Platform_TA_"))
					{
						return classNetCacheByName["TAGame.PlayerStart_Platform_TA"];
					}
					else if (typeName.Contains("InMapScoreboard_TA_"))
					{
						return classNetCacheByName["TAGame.InMapScoreboard_TA"];
					}
					else if (typeName.Contains("HauntedBallTrapTrigger_TA_"))
					{
						return classNetCacheByName["TAGame.HauntedBallTrapTrigger_TA"];
					}

					return null; // This was likely not a TypeName, so don't map it
			}
		}
	}
}
