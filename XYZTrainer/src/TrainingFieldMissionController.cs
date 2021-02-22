using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;
using SandBox;
using StoryMode.StoryModeObjects;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Objects;
using TaleWorlds.ObjectSystem;

namespace XYZTrainer
{
    
    class XYZTrainerMissionController : MissionLogic
    {

        Agent PlayerAgent = null;

        public override void OnMissionActivate()
        {
        }

        public override void OnMissionDeactivate()
        {
        }
        
        public override void AfterStart()
        {
            MBDebug.Print("XYZ: XYZTrainerMissionController::AfterStart");
            base.AfterStart();


            this._color1 = 1;
            this._color2 = 2;

            base.Mission.Teams.Add(BattleSideEnum.Defender, _color1, _color2, null, true, false, true);
            base.Mission.Teams.Add(BattleSideEnum.Attacker, _color1, _color2, null, true, false, true);
            base.Mission.Teams.Add(BattleSideEnum.Attacker, uint.MaxValue, uint.MaxValue, null, true, false, true);
            base.Mission.PlayerTeam = base.Mission.AttackerTeam;
            this._playerTeam = base.Mission.AttackerTeam;
            this._enemyTeam = base.Mission.DefenderTeam;

            this._culture = Game.Current.ObjectManager.GetObject<BasicCultureObject>("empire");
            this._banner = Banner.CreateRandomBanner(-1);
            this.PlayerParty = new XYZCombatant(new TextObject("{=sSJSTe5p}Player Party", null), _culture, _banner);
            this.EnemyParty = new XYZCombatant(new TextObject("{=0xC75dN6}Enemy Party", null), _culture, _banner);

            this.PlayerAgent = SpawnPlayer();
            this._trainerAgent = SpawnTrainer();

            this.InitializeTrainingAreas();
        }

        private Agent SpawnPlayer()
        {

            MatrixFrame matrixFrame = MatrixFrame.Identity;
            matrixFrame.origin = new Vec3(324.665f, 391.142f, 8.480f);
            matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();

            BasicCharacterObject playerCharacter = GetCharacter("xyz_eox");
            AgentBuildData agentBuildData = new AgentBuildData(playerCharacter)
                .Team(base.Mission.PlayerTeam).InitialFrame(matrixFrame)
                .NoHorses(true).NoWeapons(false).ClothingColor1(base.Mission.PlayerTeam.Color)
                .ClothingColor2(base.Mission.PlayerTeam.Color2)
                .TroopOrigin(new XYZAgentOrigin(PlayerParty, playerCharacter, true))
                .Controller(Agent.ControllerType.Player);
            Agent agent = base.Mission.SpawnAgent(agentBuildData, false, 0);
            return agent;
        }

        private Agent SpawnTrainer()
        {
            this._trainerInitFrame = MatrixFrame.Identity;
            GameEntity trainerSpawner = base.Mission.Scene.FindEntityWithTag("spawner_adv_melee_npc_easy");
            this._trainerInitFrame = trainerSpawner.GetGlobalFrame();
            this._trainerInitFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
            
            BasicCharacterObject trainerCharacter = GetCharacter("xyz_blocking_trainer");
            AgentBuildData agentBuildData = new AgentBuildData(trainerCharacter)
                .Team(_playerTeam).InitialFrame(this._trainerInitFrame)
                .ClothingColor1(_color1).ClothingColor2(_color2).NoHorses(true)
                .TroopOrigin(new XYZAgentOrigin(PlayerParty, trainerCharacter, false))
                .Controller(Agent.ControllerType.AI);
            Agent agent = base.Mission.SpawnAgent(agentBuildData, false, 0);
            agent.SetTeam(_enemyTeam, false);
            return agent;
        }

        private BasicCharacterObject GetCharacter(string name)
        {
            return Game.Current.ObjectManager.GetObject<BasicCharacterObject>(name);
        }

        private void InitializeTrainingAreas()
        {
            List<GameEntity> list = new List<GameEntity>();
            Mission.Current.Scene.GetEntities(ref list);
            foreach (GameEntity gameEntity in list)
            {
                if (gameEntity.GetFirstScriptOfType<TrainingArea>() != null)
                {
                    this._trainingAreas.Add(gameEntity.GetFirstScriptOfType<TrainingArea>());
                }
            }

            // ---------------------------------
            // InitializeTrainingAreas
            // For not ended:
            //   TrainingAreaUpdate
            //     One of Three
            //     * OnTrainingAreaEnter
            //     * InTrainingArea
            //     * OnTrainingAreaExit
            // ----------------------------------
        }
        private void TrainingAreaUpdate()
        {
            
            if (this._activeTrainingArea != null)
            {
                if (this._activeTrainingArea.IsPositionInsideTrainingArea(Agent.Main.Position))
                {
                    this.InTrainingArea();
                }
                else
                {
                    this.OnTrainingAreaExit();
                    this._activeTrainingArea = null;
                }
            }
            else
            {
                foreach (TrainingArea trainingArea in this._trainingAreas)
                {
                    if (trainingArea.IsPositionInsideTrainingArea(Agent.Main.Position))
                    {
                        this._activeTrainingArea = trainingArea;
                        this.OnTrainingAreaEnter();
                        break;
                    }
                }
            }
        }


        private void OnTrainingAreaEnter()
        {
            // Do stuff
            InformationManager.DisplayMessage(new InformationMessage("Enter Training Area"));
            Mission.Current.MakeSound(
                SoundEvent.GetEventIdFromString("event:/mission/tutorial/vo/fighting/greet"),
                this._trainerAgent.GetEyeGlobalPosition(), true, false, -1, -1);
            // NPC attacks
        }
        
        private void InTrainingArea()
        {

        }

        private void EndTraining()
        {
            this._activeTrainingArea = null;
        }

        private void ResetTrainingArea()
        {
            this.OnTrainingAreaExit();
            this.OnTrainingAreaEnter();
        }

        // Token: 0x0600011F RID: 287 RVA: 0x000064F0 File Offset: 0x000046F0
        private void OnTrainingAreaExit()
        {
            InformationManager.DisplayMessage(new InformationMessage("Exit Training Area"));
            Mission.Current.MakeSound(
                SoundEvent.GetEventIdFromString("event:/mission/tutorial/finish_course"),
                Agent.Main.GetEyeGlobalPosition(), true, false, -1, -1);
        }

        public XYZCombatant PlayerParty;
        public XYZCombatant EnemyParty;

        public Banner _banner;
        private BasicCultureObject _culture;
        private Team _playerTeam;
        private Team _enemyTeam;


        private MatrixFrame _trainerInitFrame;
        private Agent _trainerAgent;

        private uint _color1;
        private uint _color2;

        private TrainingArea _activeTrainingArea;

        private List<TrainingArea> _trainingAreas = new List<TrainingArea>();
    }
}
