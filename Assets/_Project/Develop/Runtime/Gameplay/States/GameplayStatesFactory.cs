using _Project.Develop.Runtime.Gameplay.Features.InputFeature;
using _Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using _Project.Develop.Runtime.Gameplay.Features.Station;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Utilities.Conditions;
using _Project.Develop.Runtime.Utilities.CoroutinesManagement;
using _Project.Develop.Runtime.Utilities.SceneManagement;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.States
{
    public class GameplayStatesFactory
    {
        private readonly DIContainer _container;

        public GameplayStatesFactory(DIContainer container)
        {
            _container = container;
        }

        public PreparationState CreatePreparationState()
        {
            return new PreparationState();
        }

        public StageProcessState CreateStageProcessState()
        {
            return new StageProcessState(_container.Resolve<StageProviderService>());
        }

        public WinState CreateWinState()
        {
            return new WinState(
                _container.Resolve<IInputService>(),
                _container.Resolve<SceneSwitcherService>(),
                _container.Resolve<ICoroutinesPerformer>());
        }

        public DefeatState CreateDefeatState()
        {
            return new DefeatState(
                _container.Resolve<IInputService>(),
                _container.Resolve<SceneSwitcherService>(),
                _container.Resolve<ICoroutinesPerformer>());
        }

        public GameplayStateMachine CreateCoreLoopState()
        {
            StageProviderService stageProviderService = _container.Resolve<StageProviderService>();

            StageProcessState stageProcessState = CreateStageProcessState();
            PreparationState preparationState = CreatePreparationState();

            ICondition stageToPreparationStateCondition =
                new FuncCondition(() => stageProviderService.CurrentStageResult.Value == StageResult.Completed);

            ICompositeCondition preparationToStageStateCondition = new CompositeCondition()
                .Add(new FuncCondition(() => stageProviderService.HasNextStage()))
                .Add(new FuncCondition(() => Input.GetKeyDown(KeyCode.Space)));

            GameplayStateMachine coreLoopState = new GameplayStateMachine();

            coreLoopState.AddState(stageProcessState);
            coreLoopState.AddState(preparationState);
            
            coreLoopState.AddTransition(stageProcessState, preparationState, stageToPreparationStateCondition);
            coreLoopState.AddTransition(preparationState, stageProcessState, preparationToStageStateCondition);

            return coreLoopState;
        }

        public GameplayStateMachine CreateGameplayStateMachine()
        {
            StageProviderService stageProviderService = _container.Resolve<StageProviderService>();
            StationHolderService stationHolderService = _container.Resolve<StationHolderService>();

            GameplayStateMachine coreLoopState = CreateCoreLoopState();

            DefeatState defeatState = CreateDefeatState();
            WinState winState = CreateWinState();

            ICompositeCondition coreLoopToWinStateCondition = new CompositeCondition()
                .Add(new FuncCondition(() => stageProviderService.CurrentStageResult.Value == StageResult.Completed))
                .Add(new FuncCondition(() => stageProviderService.HasNextStage() == false));

            ICompositeCondition coreLoopToDefeatStateCondition = new CompositeCondition()
                .Add(new FuncCondition(() =>
                {
                    if (stationHolderService.Station != null)
                        return stationHolderService.Station.IsDead.Value;

                    return false;
                }));

            GameplayStateMachine gameplayCycle = new GameplayStateMachine();
            
            gameplayCycle.AddState(coreLoopState);
            gameplayCycle.AddState(winState);
            gameplayCycle.AddState(defeatState);

            gameplayCycle.AddTransition(coreLoopState, winState, coreLoopToWinStateCondition);
            gameplayCycle.AddTransition(coreLoopState, defeatState, coreLoopToDefeatStateCondition);
            
            return gameplayCycle;
        }
    }
}
