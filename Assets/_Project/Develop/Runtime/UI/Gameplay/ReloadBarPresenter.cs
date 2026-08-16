using System;
using _Project.Develop.Runtime.Gameplay.Features.ExplosionAbility;
using _Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using _Project.Develop.Runtime.UI.CommonViews;
using _Project.Develop.Runtime.UI.Core;
using UnityEngine;

namespace _Project.Develop.Runtime.UI.Gameplay
{
    public class ReloadBarPresenter : IPresenter
    {
        private const string READY_TEXT = "";

        private readonly BarWithText _reloadBarView;
        private readonly ExplosionAbilityService _explosionAbilityService;
        private readonly StageProviderService _stageProviderService;

        private IDisposable _remainingTimeChangedDisposable;
        private IDisposable _stageResultChangedDisposable;

        public ReloadBarPresenter(
            BarWithText reloadBarView,
            ExplosionAbilityService explosionAbilityService,
            StageProviderService stageProviderService)
        {
            _reloadBarView = reloadBarView;
            _explosionAbilityService = explosionAbilityService;
            _stageProviderService = stageProviderService;
        }

        public BarWithText ReloadBarView => _reloadBarView;

        public void Initialize()
        {
            UpdateValue(_explosionAbilityService.CooldownRemainingTime.Value);
            UpdateVisibility(_stageProviderService.CurrentStageResult.Value);

            _remainingTimeChangedDisposable = _explosionAbilityService
                .CooldownRemainingTime
                .Subscribe(OnRemainingTimeChanged);

            _stageResultChangedDisposable = _stageProviderService
                .CurrentStageResult
                .Subscribe(OnStageResultChanged);
        }

        public void Dispose()
        {
            _remainingTimeChangedDisposable?.Dispose();
            _remainingTimeChangedDisposable = null;

            _stageResultChangedDisposable?.Dispose();
            _stageResultChangedDisposable = null;
        }

        private void OnRemainingTimeChanged(float oldValue, float newValue) => UpdateValue(newValue);

        private void OnStageResultChanged(StageResult oldValue, StageResult newValue)
            => UpdateVisibility(newValue);

        private void UpdateValue(float remainingTime)
        {
            float totalTime = _explosionAbilityService.CooldownTotalTime;
            float readyPart = Mathf.Clamp01((totalTime - remainingTime) / totalTime);

            _reloadBarView.UpdateSlider(readyPart);
            _reloadBarView.UpdateText(GetTextFor(remainingTime));
        }

        private string GetTextFor(float remainingTime)
        {
            if (remainingTime <= 0)
                return READY_TEXT;

            return remainingTime.ToString("0.0");
        }

        private void UpdateVisibility(StageResult stageResult)
            => _reloadBarView.gameObject.SetActive(stageResult == StageResult.Uncompleted);
    }
}
