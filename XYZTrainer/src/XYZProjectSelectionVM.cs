using System;
using System.Linq;

using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.CustomBattle.CustomBattle;

using TaleWorlds.MountAndBlade;

using TaleWorlds.Engine.GauntletUI;

namespace XYZTrainer
{

    // Based on CharacterCreationCultureStageVM in StoryMode.VideoModelCollection.dll

    public class XYZProjectSelectionVM : ViewModel
    {

        private readonly XYZProjectSelectionState _state;

        public string TitleText { get; }

        private string _start_button_text { get; set; }

        private string _next_button_text { get; set; }
        private string _prev_button_text { get; set; }

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

        public XYZProjectSelectionVM(XYZProjectSelectionState state)
        {
            _state = state;
            TitleText = "XYZTrainingField";
            this.NextButtonText = new TextObject("{=Rvr1bcu8}Next", null).ToString();
            this.PrevButtonText = new TextObject("{=WXAaWZVf}Previous", null).ToString();
            this.Projects = new MBBindingList<XYZProjectVM>();
            for (int i = 0; i < 6; ++i)
            {
                XYZProjectVM item = new XYZProjectVM(i, new Action<XYZProjectVM>(this.OnProjectSelection));
                this.Projects.Add(item);
            }

            this.Title = "XYZ Training Fields";
            this.Description = "Choose your training project";
            this.SelectionText = "Training Project";
            RefreshValues();
        }
        private void OnProjectSelection(XYZProjectVM selectedProject)
        {

            foreach (XYZProjectVM projVM in
                from c in this.Projects where c.IsSelected select c)
            {
                projVM.IsSelected = false;
            }
            selectedProject.IsSelected = true;
            this.CurrentSelectedProject = selectedProject;
            this.AnyItemSelected = true;
            base.OnPropertyChanged("CanAdvance");
        }

        public void ExecuteBack()
        {
            OnPreviousStage();
        }

        public void ExecuteStart()
        {
            OnNextStage();
        }

        public void OnPreviousStage()
        {
            MBGameManager.EndGame();
        }

        public void OnNextStage()
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

        public bool CanAdvance
        {
            get
            {
                return this.Projects.Any((XYZProjectVM s) => s.IsSelected);
            }
        }

        [DataSourceProperty]
        public MBBindingList<XYZProjectVM> Projects
        {
            get
            {
                return this._projects;
            }
            set
            {
                if (value != this._projects)
                {
                    this._projects = value;
                    base.OnPropertyChangedWithValue(value, "Projects");
                }
            }
        }

        [DataSourceProperty]
        public XYZProjectVM CurrentSelectedProject
        {
            get
            {
                return this._currentSelectedProject;
            }
            set
            {
                if (value != this._currentSelectedProject)
                {
                    this._currentSelectedProject = value;
                    base.OnPropertyChangedWithValue(value, "CurrentSelectedProject");
                }
            }
        }

        [DataSourceProperty]
        public string PrevButtonText
        {
            get
            {
                return this._prev_button_text;
            }
            set
            {
                if (value != this._prev_button_text)
                {
                    this._prev_button_text = value;
                    base.OnPropertyChangedWithValue(value, "PrevButtonText");
                }
            }
        }

        [DataSourceProperty]
        public string NextButtonText
        {
            get
            {
                return this._next_button_text;
            }
            set
            {
                if (value != this._next_button_text)
                {
                    this._next_button_text = value;
                    base.OnPropertyChangedWithValue(value, "NextButtonText");
                }
            }
        }

        [DataSourceProperty]
        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                if (value != this._title)
                {
                    this._title = value;
                    base.OnPropertyChangedWithValue(value, "Title");
                }
            }
        }

        [DataSourceProperty]
        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                if (value != this._description)
                {
                    this._description = value;
                    base.OnPropertyChangedWithValue(value, "Description");
                }
            }
        }

        [DataSourceProperty]
        public string SelectionText
        {
            get
            {
                return this._selectionText;
            }
            set
            {
                if (value != this._selectionText)
                {
                    this._selectionText = value;
                    base.OnPropertyChangedWithValue(value, "SelectionText");
                }
            }
        }

        [DataSourceProperty]
        public bool AnyItemSelected
        {
            get
            {
                return this._anyItemSelected;
            }
            set
            {
                if (value != this._anyItemSelected)
                {
                    this._anyItemSelected = value;
                    base.OnPropertyChangedWithValue(value, "AnyItemSelected");
                }
            }
        }

        private MBBindingList<XYZProjectVM> _projects;

        private XYZProjectVM _currentSelectedProject;

        private string _title = "";

        private string _description = "";

        private string _selectionText = "";

        private bool _anyItemSelected = false;

    }
}
