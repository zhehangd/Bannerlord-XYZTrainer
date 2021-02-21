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

            uint color = 1;
            uint color2 = 2;
            uint color3 = 1;
            uint color4 = 2;

            base.Mission.Teams.Add(BattleSideEnum.Defender, color, color2, null, true, false, true);
            base.Mission.Teams.Add(BattleSideEnum.Attacker, color3, color4, null, true, false, true);
            base.Mission.Teams.Add(BattleSideEnum.Attacker, uint.MaxValue, uint.MaxValue, null, true, false, true);
            base.Mission.PlayerTeam = base.Mission.AttackerTeam;

            MatrixFrame matrixFrame = MatrixFrame.Identity;
            matrixFrame.origin = new Vec3(324.665f, 391.142f, 8.480f);
            matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();

            BasicCharacterObject playerCharacter = Game.Current.ObjectManager.GetObject<BasicCharacterObject>("xyz_eox");

            //.MountKey(MountCreationKey.GetRandomMountKey(playerCharacter.Equipment[EquipmentIndex.ArmorItemEndSlot].Item, playerCharacter.GetMountKeySeed()))
            AgentBuildData agentBuildData = new AgentBuildData(playerCharacter);

            BasicCultureObject MainCulture = Game.Current.ObjectManager.GetObject<BasicCultureObject>("empire");
            if (MainCulture is null)
            {
                MBDebug.Print("Couldn't find Empire culture, use an empty one.");
                MainCulture = new BasicCultureObject();
            }
            PlayerParty = new XYZCombatant(new TextObject("{=sSJSTe5p}Player Party", null), MainCulture, Banner.CreateRandomBanner(-1));
            EnemyParty = new XYZCombatant(new TextObject("{=0xC75dN6}Enemy Party", null), MainCulture, Banner.CreateRandomBanner(-1));

            agentBuildData = agentBuildData
                .Team(base.Mission.PlayerTeam).InitialFrame(matrixFrame)
                .NoHorses(true).NoWeapons(false).ClothingColor1(base.Mission.PlayerTeam.Color);
            agentBuildData = agentBuildData
                .ClothingColor2(base.Mission.PlayerTeam.Color2)
                .TroopOrigin(new XYZAgentOrigin(PlayerParty, playerCharacter, true));
            agentBuildData = agentBuildData
                .Controller(Agent.ControllerType.Player);
            Agent agent = base.Mission.SpawnAgent(agentBuildData, false, 0);
            this.PlayerAgent = agent;
        }

        public XYZCombatant PlayerParty;
        public XYZCombatant EnemyParty;
    }
}
