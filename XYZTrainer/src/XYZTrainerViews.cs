﻿using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.LegacyGUI.Missions;
using TaleWorlds.MountAndBlade.Missions.Handlers;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.Missions;
using TaleWorlds.MountAndBlade.ViewModelCollection;

namespace XYZTrainer
{
    [ViewCreatorModule]
    class XYZTrainerViews
    {
		[ViewMethod("XTZTrainingField")]
		public static MissionView[] OpenCustomBattleMission(Mission mission)
		{
			return new List<MissionView>
			{
				ViewCreator.CreateMissionSingleplayerEscapeMenu(),
				ViewCreator.CreateOptionsUIHandler(),
				ViewCreator.CreateMissionAgentStatusUIHandler(mission),
				ViewCreator.CreateMissionMainAgentEquipmentController(mission),
			}.ToArray();
		}
	}
}
