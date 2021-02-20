
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.Screen;

namespace XYZTrainer.UI
{
    [GameStateScreen(typeof(XYZTrainerState))]
    public class XYZTrainerMainScreen : ScreenBase, IGameStateListener
    {
        private XYZTrainerVM _dataSource;
        private GauntletMovie _gauntletMovie;
        private GauntletLayer _gauntletLayer;
        private bool _isMovieLoaded;
        private XYZTrainerState _state;

        public XYZTrainerMainScreen(XYZTrainerState state) {
            this._state = state;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _dataSource = new XYZTrainerVM(this._state);
            _gauntletLayer = new GauntletLayer(1, "GauntletLayer");
            this.LoadMovie();
            _gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
            AddLayer(_gauntletLayer);
            MBDebug.Print("StartButtonText: " + _dataSource.StartButtonText);
            _dataSource.OnPropertyChangedWithValue(_dataSource.StartButtonText, "StartButtonText");
        }
        protected override void OnFinalize()
        {
            this.UnloadMovie();
            base.RemoveLayer(this._gauntletLayer);
            this._dataSource = null;
            this._gauntletLayer = null;
            base.OnFinalize();
        }

        protected override void OnActivate()
        {
            this.LoadMovie();
            XYZTrainerVM dataSource = this._dataSource;
            this._gauntletLayer.IsFocusLayer = true;
            ScreenManager.TrySetFocus(this._gauntletLayer);
            LoadingWindow.DisableGlobalLoadingWindow();
            base.OnActivate();
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();
            this.UnloadMovie();
        }

        private void LoadMovie()
        {
            if (!this._isMovieLoaded)
            {
                this._gauntletMovie = this._gauntletLayer.LoadMovie(nameof(XYZTrainerMainScreen), this._dataSource);
                this._isMovieLoaded = true;
            }
        }

        // Token: 0x060000C0 RID: 192 RVA: 0x00007610 File Offset: 0x00005810
        private void UnloadMovie()
        {
            if (this._isMovieLoaded)
            {
                this._gauntletLayer.ReleaseMovie(this._gauntletMovie);
                this._gauntletMovie = null;
                this._isMovieLoaded = false;
                this._gauntletLayer.IsFocusLayer = false;
                ScreenManager.TryLoseFocus(this._gauntletLayer);
            }
        }

        void IGameStateListener.OnActivate()
        {
        }

        // Token: 0x060000B7 RID: 183 RVA: 0x00007395 File Offset: 0x00005595
        void IGameStateListener.OnDeactivate()
        {
        }

        // Token: 0x060000B8 RID: 184 RVA: 0x00007397 File Offset: 0x00005597
        void IGameStateListener.OnInitialize()
        {
        }

        // Token: 0x060000B9 RID: 185 RVA: 0x00007399 File Offset: 0x00005599
        void IGameStateListener.OnFinalize()
        {
            this._dataSource.OnFinalize();
        }
    }
}
