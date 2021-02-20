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
                (new InitialStateOption("XYZTrainingField",
                new TextObject("XYZ Training Field"), 3,
                () => {MBGameManager.StartNewGame(new XYZTrainerGameManager());}, false));
        }
    }
}
