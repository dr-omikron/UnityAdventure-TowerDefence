using System;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.UI.Core;

namespace _Project.Develop.Runtime.UI.Gameplay.HealthBars
{
    public class HealthBarPresenter : IPresenter
    {
        private readonly HealthBarView _healthBarView;
        private readonly Entity _entity;

        private IDisposable _healthChangedDisposable;

        public HealthBarPresenter(HealthBarView healthBarView, Entity entity)
        {
            _healthBarView = healthBarView;
            _entity = entity;
        }

        public HealthBarView HealthBarView => _healthBarView;

        public void Initialize()
        {
            _healthBarView.SetTarget(_entity.Transform);

            UpdateValue(_entity.CurrentHealth.Value);

            _healthChangedDisposable = _entity.CurrentHealth.Subscribe(OnHealthChanged);
        }

        public void Dispose()
        {
            _healthChangedDisposable?.Dispose();
            _healthChangedDisposable = null;
        }

        private void OnHealthChanged(float oldValue, float newValue) => UpdateValue(newValue);

        private void UpdateValue(float currentHealth)
            => _healthBarView.UpdateValue(currentHealth, _entity.MaxHealth.Value);
    }
}
