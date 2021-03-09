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
    interface XYZTrainingAreaInterface
    {
        void Initialize(XYZTrainingMissionController mainCtrl);

        void OnTrainingAreaEnter();

        void OnTrainingAreaExit();

        void InTrainingArea();

        void OnScoreHit(
            Agent affectedAgent, Agent affectorAgent, WeaponComponentData attackerWeapon,
            bool isBlocked, float damage, float movementSpeedDamageModifier, float hitDistance,
            AgentAttackType attackType, float shotDifficulty, BoneBodyPartType victimHitBodyPart);

        bool IsPositionInside(Vec3 pos);

    }
}
