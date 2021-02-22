using System;
using TaleWorlds.Core;

namespace XYZTrainer
{
	// Token: 0x0200018A RID: 394
	public class XYZAgentOrigin : IAgentOriginBase
	{
		public XYZCombatant CustomBattleCombatant { get; private set; }

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x0600133D RID: 4925 RVA: 0x0004842A File Offset: 0x0004662A
		IBattleCombatant IAgentOriginBase.BattleCombatant
		{
			get
			{
				return this.CustomBattleCombatant;
			}
		}

		public BasicCharacterObject Troop { get; private set; }

		public int Rank { get; private set; }

		public Banner Banner
		{
			get
			{
				return this.CustomBattleCombatant.Banner;
			}
		}

		public bool IsUnderPlayersCommand
		{
			get
			{
				return this._isPlayerSide;
			}
		}

		public uint FactionColor
		{
			get
			{
				return this.CustomBattleCombatant.BasicCulture.Color;
			}
		}

		public uint FactionColor2
		{
			get
			{
				return this.CustomBattleCombatant.BasicCulture.Color2;
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06001346 RID: 4934 RVA: 0x0004848D File Offset: 0x0004668D
		public int Seed
		{
			get
			{
				return this.Troop.GetDefaultFaceSeed(this.Rank);
			}
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06001347 RID: 4935 RVA: 0x000484A0 File Offset: 0x000466A0
		public int UniqueSeed
		{
			get
			{
				return this._descriptor.UniqueSeed;
			}
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x000484BC File Offset: 0x000466BC
		public XYZAgentOrigin(XYZCombatant customBattleCombatant, BasicCharacterObject characterObject, bool isPlayerSide)
		{
			int rank = -1;
			UniqueTroopDescriptor uniqueNo = default(UniqueTroopDescriptor);
			this.CustomBattleCombatant = customBattleCombatant;
			this.Troop = characterObject;
			this._descriptor = ((!uniqueNo.IsValid) ? new UniqueTroopDescriptor(Game.Current.NextUniqueTroopSeed) : uniqueNo);
			this.Rank = ((rank == -1) ? MBRandom.RandomInt(10000) : rank);
			//this._troopSupplier = troopSupplier;
			this._isPlayerSide = isPlayerSide;
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x00048527 File Offset: 0x00046727
		public void SetWounded()
		{
			if (!this._isRemoved)
			{
				//this._troopSupplier.OnTroopWounded();
				this._isRemoved = true;
			}
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x00048543 File Offset: 0x00046743
		public void SetKilled()
		{
			if (!this._isRemoved)
			{
				//this._troopSupplier.OnTroopKilled();
				this._isRemoved = true;
			}
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x0004855F File Offset: 0x0004675F
		public void SetRouted()
		{
			if (!this._isRemoved)
			{
				//this._troopSupplier.OnTroopRouted();
				this._isRemoved = true;
			}
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x0004857B File Offset: 0x0004677B
		public void OnAgentRemoved(float agentHealth)
		{
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x0004857D File Offset: 0x0004677D
		void IAgentOriginBase.OnScoreHit(BasicCharacterObject victim, BasicCharacterObject captain, int damage, bool isFatal, bool isTeamKill, WeaponComponentData attackerWeapon)
		{
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x0004857F File Offset: 0x0004677F
		public void SetBanner(Banner banner)
		{
			throw new NotImplementedException();
		}

		// Token: 0x040006C4 RID: 1732
		private readonly UniqueTroopDescriptor _descriptor;

		// Token: 0x040006C5 RID: 1733
		private readonly bool _isPlayerSide;

		// Token: 0x040006C6 RID: 1734
		//private CustomBattleTroopSupplier _troopSupplier;

		// Token: 0x040006C7 RID: 1735
		private bool _isRemoved;
	}
}
