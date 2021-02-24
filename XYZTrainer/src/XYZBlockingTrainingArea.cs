using System;
using System.Collections.Generic;

using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace XYZTrainer
{
    class XYZBlockingTrainingArea
    {
        public XYZBlockingTrainingArea Initialize()
        {
            this.FindArea();
            this.SpawnTrainer();
            return this;
        }

        public void OnTrainingAreaEnter()
        {
            this._trainerAgent.SetTeam(Mission.Current.PlayerEnemyTeam, false);
            var comp = this._trainerAgent.GetComponent<AgentAIStateFlagComponent>();
            comp.CurrentWatchState = AgentAIStateFlagComponent.WatchState.Alarmed;
            this._trainerAgent.DisableScriptedMovement();
        }

        public void OnTrainingAreaExit()
        {
            InformationManager.DisplayMessage(new InformationMessage("Exit Training Area"));
            //Mission.Current.MakeSound(
            //    SoundEvent.GetEventIdFromString("event:/mission/tutorial/finish_course"),
            //    Agent.Main.GetEyeGlobalPosition(), true, false, -1, -1);
        }

        public void InTrainingArea()
        {
            //this.CurrentObjectiveTick(new TextObject("{=yflx4LNc}Defeat the trainer!", null));
        }

        public bool IsPositionInside(Vec3 pos)
        {
            return this._area_entity.IsPositionInsideTutorialArea(pos);
        }

        private void SpawnTrainer()
        {
            var mission = Mission.Current;
            var mainLogic = mission.GetMissionBehaviour<XYZTrainerMissionController>();
            this._trainerInitFrame = MatrixFrame.Identity;
            GameEntity trainerSpawner = mission.Scene.FindEntityWithTag("spawner_adv_melee_npc_easy");
            this._trainerInitFrame = trainerSpawner.GetGlobalFrame();
            this._trainerInitFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();

            BasicCharacterObject trainerCharacter = Game.Current.ObjectManager.GetObject<BasicCharacterObject>("xyz_blocking_trainer");
            AgentBuildData agentBuildData = new AgentBuildData(trainerCharacter)
                .Team(mission.PlayerTeam).InitialFrame(this._trainerInitFrame)
                .ClothingColor1(mission.PlayerTeam.Color)
                .ClothingColor2(mission.PlayerTeam.Color2).NoHorses(true)
                .TroopOrigin(new XYZAgentOrigin(mainLogic.MainCombatant, trainerCharacter, false))
                .Controller(Agent.ControllerType.AI);
            Agent agent = mission.SpawnAgent(agentBuildData, false, 0);
            //agent.SetTeam(_enemyTeam, false);
            this._trainerAgent = agent;
        }

        private void FindArea()
        {
            this._area_entity = null;
            List<GameEntity> list = new List<GameEntity>();
            Mission.Current.Scene.GetEntities(ref list);
            foreach (GameEntity gameEntity in list)
            {
                var area = gameEntity.GetFirstScriptOfType<TutorialArea>();
                if (area != null && area.TypeOfTraining == TutorialArea.TrainingType.AdvancedMelee)
                {
                    this._area_entity = area;
                    break;
                }
            }
            if (this._area_entity is null)
            {
                throw new XYZException("Couldn't find a TutorialArea for blocking training.");
            }
        }

        private MatrixFrame _trainerInitFrame;
        private Agent _trainerAgent;

        private TutorialArea _area_entity;
    }
}
