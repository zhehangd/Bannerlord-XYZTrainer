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
            base.Mission.Teams.Add(BattleSideEnum.Attacker, uint.MaxValue, uint.MaxValue, null, true, false, true); // Ally
            base.Mission.PlayerTeam = base.Mission.AttackerTeam;
            this._playerTeam = base.Mission.AttackerTeam;
            this._enemyTeam = base.Mission.DefenderTeam;

            this._culture = Game.Current.ObjectManager.GetObject<BasicCultureObject>("empire");
            this._banner = Banner.CreateRandomBanner(-1);
            this.PlayerParty = new XYZCombatant(new TextObject("{=sSJSTe5p}Player Party", null), _culture, _banner);
            this.EnemyParty = new XYZCombatant(new TextObject("{=0xC75dN6}Enemy Party", null), _culture, _banner);

            this.PlayerAgent = SpawnPlayer();
            this._trainerAgent = SpawnTrainer();

            this.InitializeTutorialAreas();
            this._report_tick = true;
            this._next_report_time = 0;
            this._total_time = 0;

            base.Mission.SetMissionMode(MissionMode.Battle, true);
            InformationManager.DisplayMessage(new InformationMessage("Initialized"));
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

        public override void OnAgentCreated(Agent agent)
        {
            // From AgentTownAILogic
            // Confirmed AgentAIStateFlagComponent is used
            base.OnAgentCreated(agent);
            if (agent.IsAIControlled)
            {
                agent.AddComponent(new UseObjectAgentComponent(agent));
                agent.AddComponent(new AgentAIStateFlagComponent(agent));
                agent.AddComponent(new AIBehaviorComponent(agent));
            }
        }

        private BasicCharacterObject GetCharacter(string name)
        {
            return Game.Current.ObjectManager.GetObject<BasicCharacterObject>(name);
        }

        private void InitializeTutorialAreas()
        {
            int numTrainingAreas = 0;
            List<GameEntity> list = new List<GameEntity>();
            Mission.Current.Scene.GetEntities(ref list);
            foreach (GameEntity gameEntity in list)
            {
                if (gameEntity.GetFirstScriptOfType<TutorialArea>() != null)
                {
                    var trainingArea = gameEntity.GetFirstScriptOfType<TutorialArea>();
                    this._trainingAreas.Add(trainingArea);
                    numTrainingAreas += 1;
                }
            }

            // ---------------------------------
            // InitializeTutorialAreas
            // For each tick:
            //   TutorialAreaUpdate
            //     One of Three
            //     * OnTutorialAreaEnter
            //     * InTutorialArea
            //     * OnTutorialAreaExit
            // ----------------------------------
        }

        public override void OnMissionTick(float dt)
        {
            _total_time += dt;
            _next_report_time -= dt;
            if (_next_report_time < 0)
            {
                _report_tick = true;
                InformationManager.DisplayMessage(new InformationMessage(string.Format("Report ({0}/{1})", dt, _total_time)));
            }

            if (_report_tick && base.Mission.MainAgent != null)
            {
                InformationManager.DisplayMessage(new InformationMessage("Agent Position: " + Agent.Main.Position));
            }
            if (base.Mission.MainAgent != null && base.Mission.MainAgent.IsActive()) {
                this.TutorialAreaUpdate();
            }
            if (_report_tick)
            {
                _next_report_time = 10;
                _report_tick = false;
            }
        }

        private void TutorialAreaUpdate()
        {
            if (this._activeTutorialArea != null)
            {
                if (this._activeTutorialArea.IsPositionInsideTutorialArea(Agent.Main.Position))
                {
                    this.InTutorialArea();
                }
                else
                {
                    this.OnTutorialAreaExit();
                    this._activeTutorialArea = null;
                }
            }
            else
            {
                foreach (TutorialArea trainingArea in this._trainingAreas)
                {
                    if (trainingArea.IsPositionInsideTutorialArea(Agent.Main.Position))
                    {
                        this._activeTutorialArea = trainingArea;
                        this.OnTutorialAreaEnter();
                        break;
                    }
                }
            }
        }


        private void OnTutorialAreaEnter()
        {
            // Do stuff
            InformationManager.DisplayMessage(new InformationMessage("Enter Training Area"));
            Mission.Current.MakeSound(
                SoundEvent.GetEventIdFromString("event:/mission/tutorial/vo/fighting/greet"),
                this._trainerAgent.GetEyeGlobalPosition(), true, false, -1, -1);
            // NPC attacks

            if (this._activeTutorialArea.TypeOfTraining == TutorialArea.TrainingType.AdvancedMelee)
            {
                OnEnterBlockingArea();
            }
        }
        
        private void InTutorialArea()
        {
            if (this._activeTutorialArea.TypeOfTraining == TutorialArea.TrainingType.AdvancedMelee)
            {
                InBlockingArea();
            }
        }

        private void OnEnterBlockingArea()
        {
            this._trainerAgent.SetTeam(Mission.Current.PlayerEnemyTeam, false);
            var comp = this._trainerAgent.GetComponent<AgentAIStateFlagComponent>();
            comp.CurrentWatchState = AgentAIStateFlagComponent.WatchState.Alarmed;
            this._trainerAgent.DisableScriptedMovement();
        }

        private void InBlockingArea()
        {
            //this.CurrentObjectiveTick(new TextObject("{=yflx4LNc}Defeat the trainer!", null));
        }

        private void EndTraining()
        {
            this._activeTutorialArea = null;
        }

        private void ResetTutorialArea()
        {
            this.OnTutorialAreaExit();
            this.OnTutorialAreaEnter();
        }

        // Token: 0x0600011F RID: 287 RVA: 0x000064F0 File Offset: 0x000046F0
        private void OnTutorialAreaExit()
        {
            InformationManager.DisplayMessage(new InformationMessage("Exit Training Area"));
            //Mission.Current.MakeSound(
            //    SoundEvent.GetEventIdFromString("event:/mission/tutorial/finish_course"),
            //    Agent.Main.GetEyeGlobalPosition(), true, false, -1, -1);
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

        private TutorialArea _activeTutorialArea;

        private List<TutorialArea> _trainingAreas = new List<TutorialArea>();

        private bool _report_tick;
        private float _total_time;
        private float _next_report_time;

        public Action<TextObject> CurrentObjectiveTick;
    }
}
