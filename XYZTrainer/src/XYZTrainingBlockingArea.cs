﻿using System;
using System.IO;
using System.Collections.Generic;

using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace XYZTrainer
{
    class XYZTrainingBlockingArea : XYZTrainingArea
    {
        public override void Initialize(XYZTrainingMissionController mainCtrl)
        {
            this._mainCtrl = mainCtrl;
            this.FindArea();
            this.SpawnTrainer();
        }

        override protected void ActivateArea()
        {
            this._progress = Progress.Standby;
        }

        override protected void DeactivateArea()
        {
            InformationManager.DisplayMessage(new InformationMessage("Exit Training Area"));
            Mission.Current.MakeSound(
                SoundEvent.GetEventIdFromString("event:/mission/tutorial/vo/fighting/player_lose"),
                this._trainerAgent.GetEyeGlobalPosition(), true, false, -1, -1);
            Agent.Main.Health = Agent.Main.HealthLimit;
            ResetTrainer();
            this._progress = Progress.Inactive;
        }

        public override void InTrainingArea()
        {
            if (_progress == Progress.Inactive)
            {
                this._progress = Progress.Standby;
            } else if (_progress == Progress.Standby)
            {
                if ((_trainerAgent.Position - Agent.Main.Position).LengthSquared < 20f)
                {
                    InformationManager.DisplayMessage(new InformationMessage("Training Starts"));
                    this._playerHealth = Agent.Main.HealthLimit;
                    this._trainerHealth = this._trainerAgent.HealthLimit;
                    InformationManager.DisplayMessage(new InformationMessage("Player Health: " + _playerHealth));
                    InformationManager.DisplayMessage(new InformationMessage("Trainer Health: " + _trainerHealth));
					ClearFightScore();
                    ActivateTrainer();
                    _progress = Progress.Fight;
                }
            } else if (this._progress == Progress.Fight)
            {
                bool playerLost = this._playerHealth <= 1f;
                bool playerWon = this._trainerHealth <= 1f;

                if (!playerLost && !playerWon) {return;}

                if (playerLost)
                {
                    Mission.Current.MakeSound(
                        SoundEvent.GetEventIdFromString("event:/mission/tutorial/vo/fighting/player_lose"),
                        this._trainerAgent.GetEyeGlobalPosition(), true, false, -1, -1);
                    AgentFallBackRise(Agent.Main);
                    Agent.Main.Health = Agent.Main.HealthLimit;
                }
                if (playerWon)
                {
                    Mission.Current.MakeSound(
                        SoundEvent.GetEventIdFromString("event:/mission/tutorial/finish_course"),
                        Agent.Main.GetEyeGlobalPosition(), true, false, -1, -1);
                    AgentFallBackRise(_trainerAgent);
                }
				ConcludeFight();
                ResetTrainer();
                StartCooldown(); // progress = Cooldown 
                return;
            } else if (this._progress == Progress.Cooldown)
            {

            } else
            {

            }

            //this.CurrentObjectiveTick(new TextObject("{=yflx4LNc}Defeat the trainer!", null));
        }
		
		public void ClearFightScore()
		{
			_blockedDamage = 0;
			_unblockedDamage = 0;
		}
		
		public void ConcludeFight()
		{
			float score = 100 * _blockedDamage / (_unblockedDamage + 100f);
			_totalScore += score;
			_numTimes += 1;
			float avgScore = _totalScore / _numTimes;
			InformationManager.DisplayMessage(new InformationMessage(String.Format("Score/Average: {0}/{1}", score, avgScore)));

            string scoreFile = _mainCtrl.SaveDirectory + "blocking_score.txt";
            String missionTimestamp = _mainCtrl.MissionTimestamp;
            String fightTimestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            File.AppendAllText(scoreFile, string.Format(
                "{0}\t{1}\t{2}\t{3}\t{4}\n", missionTimestamp, fightTimestamp,
                score, _blockedDamage, _unblockedDamage));
            ClearFightScore();
		}

        public void StartCooldown()
        {
            _progress = Progress.Cooldown;
            ResetTrainer();
            _mainCtrl.AddDelayedAction(delegate () {
                _progress = Progress.Inactive;
                InformationManager.DisplayMessage(new InformationMessage("Training Field Reset"));
            }, 4);
        }

        public void AgentFallBackRise(Agent agent)
        {
            agent.SetActionChannel(0, this.FallBackRiseAnimation, false, 0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
        }

        public void ResetTrainer()
        {
            WorldPosition worldPosition = this._trainerInitFrame.origin.ToWorldPosition();
            _trainerAgent.SetScriptedPositionAndDirection(
                ref worldPosition, this._trainerInitFrame.rotation.f.AsVec2.RotationInRadians,
                true, Agent.AIScriptedFrameFlags.None, "");
            _trainerAgent.SetTeam(Mission.Current.PlayerAllyTeam, false);
            var comp = _trainerAgent.GetComponent<AgentAIStateFlagComponent>();
            comp.CurrentWatchState = AgentAIStateFlagComponent.WatchState.Patroling;
        }

        public void ActivateTrainer()
        {
            _trainerAgent.DisableScriptedMovement();
            _trainerAgent.SetTeam(Mission.Current.PlayerEnemyTeam, false);
            var comp = this._trainerAgent.GetComponent<AgentAIStateFlagComponent>();
            comp.CurrentWatchState = AgentAIStateFlagComponent.WatchState.Alarmed;
        }
		
        public override void OnScoreHit(Agent affectedAgent, Agent affectorAgent, WeaponComponentData attackerWeapon,
                              bool isBlocked, float damage, float movementSpeedDamageModifier, float hitDistance,
                              AgentAttackType attackType, float shotDifficulty, BoneBodyPartType victimHitBodyPart)
        {
            if (this._progress != Progress.Fight) { return; }
            if (affectedAgent.Controller == Agent.ControllerType.Player)
            {
                
                if (isBlocked)
                {
                    _blockedDamage += damage;
                } else
                {
                    _unblockedDamage += damage;
                    this._playerHealth -= damage;
                    affectedAgent.Health = Math.Max(1f, this._playerHealth);
                }
            } else if (affectedAgent == this._trainerAgent)
            {
                InformationManager.DisplayMessage(new InformationMessage("Trianer hurt"));
                this._trainerHealth -= damage;
            } else
            {

            }
        }

        public override bool IsPositionInside(Vec3 pos)
        {
            return this._area_entity.IsPositionInsideTutorialArea(pos);
        }

        private void SpawnTrainer()
        {
            var mission = Mission.Current;
            this._trainerInitFrame = MatrixFrame.Identity;
            GameEntity trainerSpawner = mission.Scene.FindEntityWithTag("spawner_adv_melee_npc_easy");
            this._trainerInitFrame = trainerSpawner.GetGlobalFrame();
            this._trainerInitFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();

            BasicCharacterObject trainerCharacter = Game.Current.ObjectManager.GetObject<BasicCharacterObject>("xyz_blocking_trainer");
            AgentBuildData agentBuildData = new AgentBuildData(trainerCharacter)
                .Team(mission.PlayerAllyTeam).InitialFrame(this._trainerInitFrame)
                .ClothingColor1(mission.PlayerTeam.Color)
                .ClothingColor2(mission.PlayerTeam.Color2).NoHorses(true)
                .TroopOrigin(new XYZAgentOrigin(_mainCtrl.MainCombatant, trainerCharacter, false))
                .Controller(Agent.ControllerType.AI);
            Agent agent = mission.SpawnAgent(agentBuildData, false, 0);
            agent.SetInvulnerable(true);
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

        private ActionIndexCache FallBackRiseAnimation = ActionIndexCache.Create("act_strike_fall_back_back_rise");

        // Token: 0x040000CC RID: 204
        private ActionIndexCache FallBackRiseAnimationContinue = ActionIndexCache.Create("act_strike_fall_back_back_rise_continue");

        enum Progress
        {
            Inactive,
            Standby,
            Countdown,
            Fight,
            Cooldown,
        }

        private Progress _progress = Progress.Inactive;

        private XYZTrainingMissionController _mainCtrl;

        private float _playerHealth = 0;
        private float _trainerHealth = 0;
		
		private float _blockedDamage = 0;
		private float _unblockedDamage = 0;
		private float _totalScore = 0;
		private int _numTimes = 0;
		
    }
}
