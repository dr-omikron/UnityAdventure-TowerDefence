using System;
using _Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using _Project.Develop.Runtime.UI.CommonViews;
using _Project.Develop.Runtime.UI.Core;
using UnityEngine;

namespace _Project.Develop.Runtime.UI.Gameplay
{
    public class StagesPresenter : IPresenter
    {
        private const int FIRST_STAGE_NUMBER = 1;

        private readonly IconTextView _stagesView;
        private readonly StageProviderService _stageProviderService;

        private IDisposable _stageNumberChangedDisposable;

        public StagesPresenter(IconTextView stagesView, StageProviderService stageProviderService)
        {
            _stagesView = stagesView;
            _stageProviderService = stageProviderService;
        }

        public void Initialize()
        {
            UpdateValue(_stageProviderService.CurrentStageNumber.Value);

            _stageNumberChangedDisposable = _stageProviderService
                .CurrentStageNumber
                .Subscribe(OnStageNumberChanged);
        }

        public void Dispose()
        {
            _stageNumberChangedDisposable?.Dispose();
            _stageNumberChangedDisposable = null;
        }

        private void OnStageNumberChanged(int oldValue, int newValue) => UpdateValue(newValue);

        private void UpdateValue(int stageNumber)
        {
            int currentStageNumber = Mathf.Max(stageNumber, FIRST_STAGE_NUMBER);
            int stagesCount = _stageProviderService.StageCount;

            _stagesView.SetText($"{currentStageNumber}/{stagesCount}");
        }
    }
}
