using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace XYZTrainer
{
	public class XYZProjectVM : ViewModel
	{
		public XYZProjectVM(int id, Action<XYZProjectVM> onSelection)
		{
			this._onSelection = onSelection;
			MBTextManager.SetTextVariable("FOCUS_VALUE", 1);
			MBTextManager.SetTextVariable("EXP_VALUE", 10);

			switch (id)
            {
				case 0:
					this.CultureID = "vlandia";
					this.ShortenedNameText = "crossbow";
					this.NameText = "Crossbow Training";
					this.DescriptionText = "Trains your crossbow technique";
					this.PositiveEffectText = "";
					break;
				case 1:
					this.CultureID = "sturgia";
					this.ShortenedNameText = "kicking";
					this.NameText = "Kick Training";
					this.DescriptionText = "Trains your kicking technique";
					this.PositiveEffectText = "";
					break;
				case 2:
					this.CultureID = "empire";
					this.ShortenedNameText = "blocking";
					this.NameText = "Blocking Training";
					this.DescriptionText = "Trains your blocking technique";
					this.PositiveEffectText = "";
					break;
				case 3:
					this.CultureID = "aserai";
					this.ShortenedNameText = "javelin";
					this.NameText = "Javelin Training";
					this.DescriptionText = "Trains your blocking technique";
					this.PositiveEffectText = "";
					break;
				case 4:
					this.CultureID = "khuzait";
					this.ShortenedNameText = "riding";
					this.NameText = "Riding Training";
					this.DescriptionText = "Trains your riding technique";
					this.PositiveEffectText = "";
					break;
				case 5:
					this.CultureID = "battania";
					this.ShortenedNameText = "bow";
					this.NameText = "Bow Training";
					this.DescriptionText = "Trains your bow technique";
					this.PositiveEffectText = "";
					break;
				default:
					break;
			}
		}

		public void ExecuteSelectCulture()
		{
			this._onSelection(this);
		}

		[DataSourceProperty]
		public string CultureID
		{
			get
			{
				return this._cultureID;
			}
			set
			{
				if (value != this._cultureID)
				{
					this._cultureID = value;
					base.OnPropertyChangedWithValue(value, "CultureID");
				}
			}
		}

		[DataSourceProperty]
		public string PositiveEffectText
		{
			get
			{
				return this._positiveEffectText;
			}
			set
			{
				if (value != this._positiveEffectText)
				{
					this._positiveEffectText = value;
					base.OnPropertyChangedWithValue(value, "PositiveEffectText");
				}
			}
		}

		[DataSourceProperty]
		public string NegativeEffectText
		{
			get
			{
				return this._negativeEffectText;
			}
			set
			{
				if (value != this._negativeEffectText)
				{
					this._negativeEffectText = value;
					base.OnPropertyChangedWithValue(value, "NegativeEffectText");
				}
			}
		}

		[DataSourceProperty]
		public string DescriptionText
		{
			get
			{
				return this._descriptionText;
			}
			set
			{
				if (value != this._descriptionText)
				{
					this._descriptionText = value;
					base.OnPropertyChangedWithValue(value, "DescriptionText");
				}
			}
		}

		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue(value, "NameText");
				}
			}
		}

		[DataSourceProperty]
		public string ShortenedNameText
		{
			get
			{
				return this._shortenedNameText;
			}
			set
			{
				if (value != this._shortenedNameText)
				{
					this._shortenedNameText = value;
					base.OnPropertyChangedWithValue(value, "ShortenedNameText");
				}
			}
		}

		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		private readonly Action<XYZProjectVM> _onSelection;

		private string _descriptionText = "";

		private string _nameText;

		private string _shortenedNameText;

		private bool _isSelected;

		private string _cultureID;

		private string _positiveEffectText;

		private string _negativeEffectText;
	}
}
