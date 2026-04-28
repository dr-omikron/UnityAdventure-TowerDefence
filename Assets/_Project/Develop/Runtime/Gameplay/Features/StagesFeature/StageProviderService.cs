using System;
using _Project.Develop.Runtime.Configs.Gameplay.Levels;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Utilities.Reactive;

namespace _Project.Develop.Runtime.Gameplay.Features.StagesFeature
{
    public class StageProviderService : IDisposable
    {
        private readonly ReactiveVariable<int> _currentStageNumber = new ReactiveVariable<int>();
        private readonly ReactiveVariable<StageResult> _currentStageResult = new ReactiveVariable<StageResult>();
        private readonly LevelConfig _levelConfig;
        private readonly StagesFactory _stageFactory;
        private IStage _currentStage;

        private IDisposable _stageEndedDisposable;

        public StageProviderService(LevelConfig levelConfig, StagesFactory stageFactory)
        {
            _levelConfig = levelConfig;
            _stageFactory = stageFactory;
        }

        public IReadOnlyVariable<int> CurrentStageNumber => _currentStageNumber;
        public IReadOnlyVariable<StageResult> CurrentStageResult => _currentStageResult;
        public int StageCount => _levelConfig.StageConfigs.Count;

        public bool HasNextStage() => CurrentStageNumber.Value < StageCount;

        public void SwitchToNext()
        {
            if (HasNextStage() == false)
                throw new InvalidOperationException("No more stages available");

            if (_currentStage != null)
                CleanupCurrent();

            _currentStageNumber.Value++;
            _currentStageResult.Value = StageResult.Uncompleted;
            _currentStage = _stageFactory.Create(_levelConfig.StageConfigs[_currentStageNumber.Value - 1]);
        }

        public void StartCurrent()
        {
            _stageEndedDisposable = _currentStage.Completed.Subscribe(OnStageCompleted);
            _currentStage.Start();
        }

        public void UpdateCurrent(float deltaTime) => _currentStage.Update(deltaTime);

        public void CleanupCurrent() => _currentStage.Cleanup();

        public void Dispose()
        {
            _currentStage?.Dispose();
            _stageEndedDisposable?.Dispose();
        }

        private void OnStageCompleted() => _currentStageResult.Value = StageResult.Completed;
    }
}
