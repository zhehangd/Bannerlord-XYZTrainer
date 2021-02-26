using System;
using System.IO;
using System.Collections.Generic;

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
    
    class XYZTrainingMissionController : MissionLogic
    {

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
            this.MainCombatant = new XYZCombatant(new TextObject("{=sSJSTe5p}Player Party", null), _culture, _banner);
            //this.EnemyParty = new XYZCombatant(new TextObject("{=0xC75dN6}Enemy Party", null), _culture, _banner);

            SpawnPlayer();

            MissionTimestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            this.SaveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\Mount and Blade II Bannerlord\\XYZTrainer\\";
            if (!Directory.Exists(this.SaveDirectory))
            {
                Directory.CreateDirectory(this.SaveDirectory);
            }

            this.SpwanNPC("xyz_eox", new Vec3(294.278f, 405f, 8.310f));
            this.SpwanNPC("xyz_starryknight", new Vec3(295.278f, 405f, 8.310f));
            this.SpwanNPC("xyz_yaksha", new Vec3(293.278f, 405f, 8.310f));
            this.SpwanNPC("xyz_atlantis_164", new Vec3(292.278f, 405f, 8.310f));
            this.SpwanNPC("xyz_second", new Vec3(291.278f, 405f, 8.310f));

            this.InitializeTutorialAreas(); // after player
            this._report_tick = true;
            this._next_report_time = 0;
            this._total_time = 0;

            base.Mission.SetMissionMode(MissionMode.Battle, true);
            InformationManager.DisplayMessage(new InformationMessage("Initialized"));
        }

        private Agent SpwanNPC(string name, Vec3 pos)
        {
            MatrixFrame matrixFrame = MatrixFrame.Identity;
            matrixFrame.origin = pos;
            matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();

            BasicCharacterObject playerCharacter = GetCharacter(name);
            AgentBuildData agentBuildData = new AgentBuildData(playerCharacter)
                .Team(base.Mission.PlayerTeam).InitialFrame(matrixFrame)
                .NoHorses(true).NoWeapons(false).ClothingColor1(base.Mission.PlayerTeam.Color)
                .ClothingColor2(base.Mission.PlayerTeam.Color2)
                .TroopOrigin(new XYZAgentOrigin(MainCombatant, playerCharacter, true))
                .Controller(Agent.ControllerType.AI);
            Agent agent = base.Mission.SpawnAgent(agentBuildData, false, 0);
            return agent;
        }

        private Agent SpawnPlayer()
        {

            MatrixFrame matrixFrame = MatrixFrame.Identity;
            matrixFrame.origin = new Vec3(300f, 412f, 8.310f);
            matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();

            BasicCharacterObject playerCharacter = GetCharacter("xyz_eox");
            AgentBuildData agentBuildData = new AgentBuildData(playerCharacter)
                .Team(base.Mission.PlayerTeam).InitialFrame(matrixFrame)
                .NoHorses(true).NoWeapons(false).ClothingColor1(base.Mission.PlayerTeam.Color)
                .ClothingColor2(base.Mission.PlayerTeam.Color2)
                .TroopOrigin(new XYZAgentOrigin(MainCombatant, playerCharacter, true))
                .Controller(Agent.ControllerType.Player);
            Agent agent = base.Mission.SpawnAgent(agentBuildData, false, 0);
            agent.SetInvulnerable(true);
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
            this._trainingAreas.Add(new XYZTrainingBlockingArea().Initialize(this));

            int numTrainingAreas = 0;
            List<GameEntity> list = new List<GameEntity>();
            Mission.Current.Scene.GetEntities(ref list);
            foreach (GameEntity gameEntity in list)
            {
                if (gameEntity.GetFirstScriptOfType<TutorialArea>() != null)
                {
                    var trainingArea = gameEntity.GetFirstScriptOfType<TutorialArea>();
                    
                    numTrainingAreas += 1;
                }
            }
        }

        public override void OnMissionTick(float dt)
        {
            _total_time += dt;
            _next_report_time -= dt;
            if (_next_report_time < 0) {_report_tick = true;}

            if (_report_tick && base.Mission.MainAgent != null)
            {
                InformationManager.DisplayMessage(new InformationMessage("Agent Position: " + Agent.Main.Position));
            }
            
            TutorialAreaUpdate();
            UpdateDelayedActions();

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
                // Players may die in the training field, in that case we let training field
                // decide what to do.
                var playerAgent = base.Mission.MainAgent;
                if (playerAgent == null || this._activeTutorialArea.IsPositionInside(playerAgent.Position))
                {
                    this._activeTutorialArea.InTrainingArea();
                }
                else
                {
                    this._activeTutorialArea.OnTrainingAreaExit();
                    InactivateCurrentTrainingArea();
                }
            }
            else
            {
                foreach (var trainingArea in this._trainingAreas)
                {
                    if (trainingArea.IsPositionInside(Agent.Main.Position))
                    {
                        this._activeTutorialArea = trainingArea;
                        this._activeTutorialArea.OnTrainingAreaEnter();
                        break;
                    }
                }
            }
        }

        public override void OnScoreHit(
            Agent affectedAgent, Agent affectorAgent, WeaponComponentData attackerWeapon,
            bool isBlocked, float damage, float movementSpeedDamageModifier, float hitDistance,
            AgentAttackType attackType, float shotDifficulty, BoneBodyPartType victimHitBodyPart)
        {
            if (this._activeTutorialArea != null)
            {
                this._activeTutorialArea.OnScoreHit(
                    affectedAgent, affectorAgent, attackerWeapon, isBlocked, damage,
                    movementSpeedDamageModifier, hitDistance, attackType, shotDifficulty,
                    victimHitBodyPart);
            }
        }

        public void AddDelayedAction(Action act, float delayedTime)
        {
            _delayedActions.Add(new DelayedAction(act, delayedTime));
        }

        public void UpdateDelayedActions()
        {
            for (int i = this._delayedActions.Count - 1; i >= 0; i--)
            {
                if (this._delayedActions[i].Update())
                {
                    this._delayedActions.RemoveAt(i);
                }
            }
        }

        public void InactivateCurrentTrainingArea()
        {
            
            this._activeTutorialArea = null;
        }

        public XYZCombatant MainCombatant;

        public Banner _banner;
        private BasicCultureObject _culture;
        private Team _playerTeam;
        private Team _enemyTeam;

        private uint _color1;
        private uint _color2;

        private XYZTrainingBlockingArea _activeTutorialArea;

        private List<XYZTrainingBlockingArea> _trainingAreas = new List<XYZTrainingBlockingArea>();

        private bool _report_tick;
        private float _total_time;
        private float _next_report_time;

        // Manages an action that is planned to be executed in the future
        public struct DelayedAction
        {

            public DelayedAction(Action act, float delayedTime)
            {
                this._action = act;
                this._targetTime = Mission.Current.Time + delayedTime;
            }

            public bool Update()
            {
                if (Mission.Current.Time > this._targetTime)
                {
                    this._action();
                    return true;
                }
                return false;
            }

            private float _targetTime;

            private Action _action;
        };

        private List<DelayedAction> _delayedActions = new List<DelayedAction>();

        public String SaveDirectory;

        public String MissionTimestamp;
    }
}
