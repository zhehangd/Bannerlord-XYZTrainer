using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace XYZTrainer
{
	
	// Token: 0x0200018D RID: 397
	public class XYZCombatant : IBattleCombatant
	{
		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06001356 RID: 4950 RVA: 0x00049024 File Offset: 0x00047224
		// (set) Token: 0x06001357 RID: 4951 RVA: 0x0004902C File Offset: 0x0004722C
		public TextObject Name { get; private set; }

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001358 RID: 4952 RVA: 0x00049035 File Offset: 0x00047235
		// (set) Token: 0x06001359 RID: 4953 RVA: 0x0004903D File Offset: 0x0004723D
		public BattleSideEnum Side { get; set; }

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x0600135A RID: 4954 RVA: 0x00049046 File Offset: 0x00047246
		// (set) Token: 0x0600135B RID: 4955 RVA: 0x0004904E File Offset: 0x0004724E
		public BasicCultureObject BasicCulture { get; private set; }

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x0600135C RID: 4956 RVA: 0x00049057 File Offset: 0x00047257
		public Tuple<uint, uint> PrimaryColorPair
		{
			get
			{
				return new Tuple<uint, uint>(this.Banner.GetPrimaryColor(), this.Banner.GetPrimaryColor());
			}
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x0600135D RID: 4957 RVA: 0x00049074 File Offset: 0x00047274
		public Tuple<uint, uint> AlternativeColorPair
		{
			get
			{
				return new Tuple<uint, uint>(this.Banner.GetPrimaryColor(), this.Banner.GetPrimaryColor());
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x0600135E RID: 4958 RVA: 0x00049091 File Offset: 0x00047291
		// (set) Token: 0x0600135F RID: 4959 RVA: 0x00049099 File Offset: 0x00047299
		public Banner Banner { get; private set; }

		// Token: 0x06001360 RID: 4960 RVA: 0x000490A2 File Offset: 0x000472A2
		public int GetTacticsSkillAmount()
		{
			if (this._characters.Any<BasicCharacterObject>())
			{
				return this._characters.Max((BasicCharacterObject h) => h.GetSkillValue(DefaultSkills.Tactics));
			}
			return 0;
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06001361 RID: 4961 RVA: 0x000490DD File Offset: 0x000472DD
		public IEnumerable<BasicCharacterObject> Characters
		{
			get
			{
				return this._characters.AsReadOnly();
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06001362 RID: 4962 RVA: 0x000490EA File Offset: 0x000472EA
		// (set) Token: 0x06001363 RID: 4963 RVA: 0x000490F2 File Offset: 0x000472F2
		public int NumberOfAllMembers { get; private set; }

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06001364 RID: 4964 RVA: 0x000490FB File Offset: 0x000472FB
		public int NumberOfHealthyMembers
		{
			get
			{
				return this._characters.Count;
			}
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x00049108 File Offset: 0x00047308
		public XYZCombatant(TextObject name, BasicCultureObject culture, Banner banner)
		{
			this.Name = name;
			this.BasicCulture = culture;
			this.Banner = banner;
			this._characters = new List<BasicCharacterObject>();
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x00049130 File Offset: 0x00047330
		public void AddCharacter(BasicCharacterObject characterObject, int number)
		{
			for (int i = 0; i < number; i++)
			{
				this._characters.Add(characterObject);
			}
			this.NumberOfAllMembers += number;
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x00049163 File Offset: 0x00047363
		public void KillCharacter(BasicCharacterObject character)
		{
			this._characters.Remove(character);
		}

		// Token: 0x040006CC RID: 1740
		private List<BasicCharacterObject> _characters;
	}
	
}
