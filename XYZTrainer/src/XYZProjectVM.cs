﻿using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace XYZTrainer
{
	public class XYZProjectVM : ViewModel
	{
		public XYZProjectVM(
			string spriteCultureID, string thumbnailName, string fullname,
			string descriptionText, string highlightText,
			Action<XYZProjectVM> onSelection,
			Action onNext)
			
		{
			this._onSelection = onSelection;
			MBTextManager.SetTextVariable("FOCUS_VALUE", 1);
			MBTextManager.SetTextVariable("EXP_VALUE", 10);

			this.CultureID = spriteCultureID;
			this.ShortenedNameText = thumbnailName;
			this.NameText = fullname;
			this.DescriptionText = descriptionText;
			this.PositiveEffectText = highlightText;
			this.OnNext = onNext;
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

		public Action OnNext;

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
