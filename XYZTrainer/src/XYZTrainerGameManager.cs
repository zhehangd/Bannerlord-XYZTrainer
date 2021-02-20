using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;

namespace XYZTrainer
{
	public class XYZTrainerGameManager : MBGameManager
	{
		protected override void DoLoadingForGameManager(GameManagerLoadingSteps gameManagerLoadingStep, out GameManagerLoadingSteps nextStep)
		{
			MBDebug.Print("XYZ: DoLoadingForGameManager");
			nextStep = GameManagerLoadingSteps.None;
			switch (gameManagerLoadingStep)
			{
				case GameManagerLoadingSteps.PreInitializeZerothStep:
					MBGameManager.LoadModuleData(false);
					MBGlobals.InitializeReferences();
					Game.CreateGame(new XYZTrainerGame(), this).DoLoading(); // Offer our Game
					MBDebug.Print("XYZ: XYZTrainerGame Loaded");
					nextStep = GameManagerLoadingSteps.FirstInitializeFirstStep;
					return;
				case GameManagerLoadingSteps.FirstInitializeFirstStep:
					{
						bool flag = true;
						foreach (MBSubModuleBase mbsubModuleBase in Module.CurrentModule.SubModules)
						{
							flag = (flag && mbsubModuleBase.DoLoading(Game.Current));
						}
						nextStep = (flag ? GameManagerLoadingSteps.WaitSecondStep : GameManagerLoadingSteps.FirstInitializeFirstStep);
						return;
					}
				case GameManagerLoadingSteps.WaitSecondStep:
					MBGameManager.StartNewGame();
					MBDebug.Print("XYZ: StartNewGame Finished");
					nextStep = GameManagerLoadingSteps.SecondInitializeThirdState;
					return;
				case GameManagerLoadingSteps.SecondInitializeThirdState:
					nextStep = (Game.Current.DoLoading() ? GameManagerLoadingSteps.PostInitializeFourthState : GameManagerLoadingSteps.SecondInitializeThirdState);
					return;
				case GameManagerLoadingSteps.PostInitializeFourthState:
					nextStep = GameManagerLoadingSteps.FinishLoadingFifthStep;
					return;
				case GameManagerLoadingSteps.FinishLoadingFifthStep:
					nextStep = GameManagerLoadingSteps.None;
					return;
				default:
					return;
			}
		}

		public override void OnLoadFinished()
		{
			base.OnLoadFinished();
			MBDebug.Print("XYZ: Pushing GameState");
			Game.Current.GameStateManager.CleanAndPushState(Game.Current.GameStateManager.CreateState<XYZTrainerState>(), 0);
			MBDebug.Print("XYZ: Pushed GameState");
		}
	}
}
