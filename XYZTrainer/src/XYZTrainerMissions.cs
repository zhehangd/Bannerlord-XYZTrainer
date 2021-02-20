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

    public static class XYZTrainerMissions
    {
        public static Mission OpenXYZTrainerMission()
        {
			string scene = "training_field_2";
			MBDebug.Print("XYZ " + Game.Current.GameStateManager);
			MissionInitializerRecord rec = CreateXYZMissionInitializerRecord(scene);
			MBDebug.Print("XYZ: MissionState.OpenNew");
			Mission createdMission = MissionState.OpenNew("XTZTrainingField", rec,
				(Mission mission) => new MissionBehaviour[] {
					new XYZTrainerMissionController(),
			}, true, true);
			//new MissionOptionsComponent(),
			//new CampaignMissionComponent(),
			//new MissionBasicTeamLogic(),
			//new XYZTrainerMissionController(),
			//new BasicLeaveMissionLogic(),
			//new LeaveMissionLogic(),
			return createdMission;
		}

		public static MissionInitializerRecord CreateXYZMissionInitializerRecord(string sceneName, string sceneLevels = "", bool doNotUseLoadingScreen = false)
		{
			return new MissionInitializerRecord(sceneName);
		}
	}
}
