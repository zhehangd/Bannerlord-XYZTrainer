using System;
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
			Mission createdMission = MissionState.OpenNew("XTZTrainingField", rec,
				(Mission mission) => new MissionBehaviour[] {
					new MissionOptionsComponent(),
					new XYZTrainingMissionController(),
			}, true, true);
			return createdMission;
		}

		public static MissionInitializerRecord CreateXYZMissionInitializerRecord(string sceneName, string sceneLevels = "", bool doNotUseLoadingScreen = false)
		{
			return new MissionInitializerRecord(sceneName);
		}
	}
}
