using System;
using System.IO;
using System.Collections.Generic;

using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace XYZTrainer
{
    abstract class XYZTrainingArea : XYZTrainingAreaInterface
    {
        abstract public void Initialize(XYZTrainingMissionController mainCtrl);

        public void OnTrainingAreaEnter()
        {
            InformationManager.DisplayMessage(new InformationMessage("Enters Training Area"));
            ActivateArea();
        }

        public void OnTrainingAreaExit()
        {
            InformationManager.DisplayMessage(new InformationMessage("Exits Training Area"));
            Agent.Main.Health = Agent.Main.HealthLimit;
            DeactivateArea();
        }

        abstract public void InTrainingArea();

        abstract public void OnScoreHit(
            Agent affectedAgent, Agent affectorAgent, WeaponComponentData attackerWeapon,
            bool isBlocked, float damage, float movementSpeedDamageModifier, float hitDistance,
            AgentAttackType attackType, float shotDifficulty, BoneBodyPartType victimHitBodyPart);
        
        abstract protected void ActivateArea();
        abstract protected void DeactivateArea();

        public virtual bool IsPositionInside(Vec3 pos)
        {
            return ((_areaPosition - pos).LengthSquared < _areaRadius);
        }

        protected void DefineAreaRange(Vec3 pos, float radius)
        {
            _areaPosition = pos;
            _areaRadius = radius;
            CreateAreaAurora(pos, radius);
        }

        protected void SetAgentWeapon(Agent agent, string[] weapons)
        {
            for (EquipmentIndex i = EquipmentIndex.WeaponItemBeginSlot; i <= EquipmentIndex.Weapon3; i = i + 1)
            {
                if (!agent.Equipment[i].IsEmpty)
                {
                    agent.RemoveEquippedWeapon(i);
                }
            }
            int totalNumWeapons = Math.Min(weapons.Length, 4);
            for (int i = 0; i < totalNumWeapons; ++i)
            {
                string weaponName = weapons[i];
                ItemObject weaponObject = Game.Current.ObjectManager.GetObject<ItemObject>(weaponName);
                MissionWeapon missionWeapon = new MissionWeapon(weaponObject, null, null);
                agent.EquipWeaponWithNewEntity(EquipmentIndex.Weapon0 + i, ref missionWeapon);
                agent.WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp);
            }
        }

        private void CreateAreaAurora(Vec3 pos, float radius)
        {
            var mission = Mission.Current;
            MatrixFrame frame = MatrixFrame.Identity;
            frame.origin = pos;
            float r = radius * .33f;
            frame.Scale(new Vec3(r, r, r));
            _beams.Add(GameEntity.Instantiate(mission.Scene, "highlight_beam_white", frame));
        }

        private Vec3 _areaPosition;
        private float _areaRadius;
        protected List<GameEntity> _beams = new List<GameEntity>();
    }
}
