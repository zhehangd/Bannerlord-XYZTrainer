using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.MountAndBlade;

namespace XYZTrainer
{
	public class TrainingArea : MissionObject
	{
		protected  override void OnInit()
		{
			base.OnInit();
			this.GatherVolumeBoxes();
		}

		public override void AfterMissionStart()
		{
		}

		private void GatherVolumeBoxes()
		{
			List<GameEntity> list = new List<GameEntity>();
			base.GameEntity.Scene.GetEntities(ref list);
			foreach (GameEntity gameEntity in list)
			{
				bool hasTagPrefix = false;
				foreach (string text in gameEntity.Tags) {
					if (text.StartsWith(this._tagPrefix))
					{
						hasTagPrefix = true;
						break;
					}
				}
				if (hasTagPrefix)
				{
					this._volumeBoxes.Add(gameEntity.GetFirstScriptOfType<VolumeBox>());
				}
			}
		}

		public bool IsPositionInsideTrainingArea(Vec3 position)
		{
			using (List<VolumeBox>.Enumerator enumerator = this._volumeBoxes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsPointIn(position))
					{
						return true;
					}
				}
			}
			return false;
		}

		[EditableScriptComponentVariable(true)]
		private TutorialArea.TrainingType _typeOfTraining;

		[EditableScriptComponentVariable(true)]
		private string _tagPrefix = "A_";

		private List<VolumeBox> _volumeBoxes = new List<VolumeBox>();

		public enum TrainingType
		{
			Bow,
			Melee,
			Mounted,
			AdvancedMelee
		}
	}
}
