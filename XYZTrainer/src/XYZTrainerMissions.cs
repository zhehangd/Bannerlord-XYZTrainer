using System;
using System.Collections.Generic;
using SandBox;
using SandBox.Source.Missions;
using SandBox.Source.Missions.Handlers;
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Source.Missions;
using TaleWorlds.MountAndBlade.Source.Missions.Handlers;

namespace XYZTrainer
{

	[MissionManager]
	public static class XYZTrainerMissions
    {
		[MissionMethod]
		public static Mission OpenXYZTrainerMission()
        {
			string scene = "training_field_2";
			MissionInitializerRecord rec = CreateXYZMissionInitializerRecord(scene);
			MBDebug.Print("XYZ: MissionState.OpenNew");
			var list = new List<XYZTrainingArea>{ new XYZTrainingBlockingArea() };
			Mission createdMission = MissionState.OpenNew("XTZTrainingField", rec,
				(Mission mission) => new MissionBehaviour[] {
					new MissionOptionsComponent(),
					new XYZTrainingMissionController(list),
			}, true, true);
			return createdMission;
		}

		[MissionMethod]
		public static Mission OpenXYZTrainerKickMission()
		{
			string scene = "training_field_2";
			MissionInitializerRecord rec = CreateXYZMissionInitializerRecord(scene);
			MBDebug.Print("XYZ: MissionState.OpenNew");
			var list = new List<XYZTrainingArea> { new XYZTrainingStaticKickingArea() };
			Mission createdMission = MissionState.OpenNew("XTZTrainingField", rec,
				(Mission mission) => new MissionBehaviour[] {
					new MissionOptionsComponent(),
					new XYZTrainingMissionController(list),
			}, true, true);
			return createdMission;
		}

		public static MissionInitializerRecord CreateXYZMissionInitializerRecord(string sceneName, string sceneLevels = "", bool doNotUseLoadingScreen = false)
		{
			return new MissionInitializerRecord(sceneName);
		}
	}
}
