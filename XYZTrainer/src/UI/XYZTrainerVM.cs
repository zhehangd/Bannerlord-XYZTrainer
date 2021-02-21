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

            this.Projects = new MBBindingList<XYZTrainerProjectVM>();
            for (int i = 0; i < 6; ++i)
            {
                XYZTrainerProjectVM item = new XYZTrainerProjectVM(i, new Action<XYZTrainerProjectVM>(this.OnProjectSelection));
                this.Projects.Add(item);
            }

            var spriteData = UIResourceManager.SpriteData;
            var a = spriteData.GetSprite("CharacterCreation\\Culture\\empire");
            MBDebug.Print("XYZ: Sprite " + a.Name);


            this.Title = "XYZ Training Fields";
            this.Description = "Choose your training project";
            this.SelectionText = "Training Project";
            OnProjectSelection(this.Projects[2]);
            RefreshValues();
        }
        private void OnProjectSelection(XYZTrainerProjectVM selectedProject)
        {

            foreach (XYZTrainerProjectVM projVM in
                from c in this.Projects where c.IsSelected select c)
            {
                projVM.IsSelected = false;
            }
            selectedProject.IsSelected = true;
            this.CurrentSelectedProject = selectedProject;
            MBDebug.Print("XYZ selectedProject = " + selectedProject.CultureID);
            this.AnyItemSelected = true;
            //CharacterCreationContent.Instance.Culture = selectedCulture.Culture;
            //CharacterCreationContent.CultureOnCondition(this._characterCreation);
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

        protected bool CanAdvanceToNextStage()
        {
            return this.Projects.Any((XYZTrainerProjectVM s) => s.IsSelected);
        }

        [DataSourceProperty]
        public MBBindingList<XYZTrainerProjectVM> Projects
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
        public XYZTrainerProjectVM CurrentSelectedProject
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

        private MBBindingList<XYZTrainerProjectVM> _projects;

        private XYZTrainerProjectVM _currentSelectedProject;

        private string _title = "";

        private string _description = "";

        private string _selectionText = "";

        private bool _anyItemSelected = false;

    }
}
