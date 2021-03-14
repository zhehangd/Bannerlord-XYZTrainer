using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;


namespace XYZTrainer
{
    public class XYZTrainerModule : MBSubModuleBase
    {
		protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            Module.CurrentModule.AddInitialStateOption
                (new InitialStateOption("XYZTrainingFields",
                new TextObject("{=5KQgvmu3Du}XYZ Training Fields"), 3,
                () => {MBGameManager.StartNewGame(new XYZTrainerGameManager());}, () => false));
        }
    }
}
