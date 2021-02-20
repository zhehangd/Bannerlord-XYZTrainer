using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.CustomBattle.CustomBattle;

using TaleWorlds.MountAndBlade;

namespace XYZTrainer.UI
{
    public class XYZTrainerVM : ViewModel
    {

        private readonly XYZTrainerState _state;

        public string TitleText { get; }

        private string _start_button_text { get; set; }

        [DataSourceProperty]
        public string StartButtonText
        {
            get
            {
                return this._start_button_text;
            }
            set
            {
                if (value != this._start_button_text)
                {
                    this._start_button_text = value;
                    base.OnPropertyChangedWithValue(value, "StartButtonText");
                }
            }
        }

        public XYZTrainerVM(XYZTrainerState state)
        {
            _state = state;
            TitleText = "XYZTrainingField";
            RefreshValues();
        }

        public void ExecuteBack()
        {
            ApplyConfig();
            MBGameManager.EndGame();
        }

        public void ExecuteStart()
        {
            MBDebug.Print("XYZ: Missions.OpenMission");
            XYZTrainerMissions.OpenXYZTrainerMission();
            MBDebug.Print("XYZ: Missions.OpenMission Done");
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            this.StartButtonText = GameTexts.FindText("str_start", null).ToString();
        }

        private bool ApplyConfig()
        {
            return true;
        }

    }
}
