using System;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using _Project.Develop.Runtime.Utilities.Reactive;

namespace _Project.Develop.Runtime.Gameplay.Features.LifeCycle
{
    public class DeathProcessTimerSystem : IInitializableSystem, IUpdateableSystem, IDisposableSystem
    {
        private ReactiveVariable<bool> _isDead;
        private ReactiveVariable<bool> _inDeathProcess;
        private ReactiveVariable<float> _initialTime;
        private ReactiveVariable<float> _currentTime;

        private IDisposable _isDeadChangedDisposable;

        public void OnInit(Entity entity)
        {
            _isDead = entity.IsDead;
            _inDeathProcess = entity.IsDeathProcess;
            _initialTime = entity.DeathProcessInitialTime;
            _currentTime = entity.DeathProcessCurrentTime;

            _isDeadChangedDisposable = _isDead.Subscribe(OnIsDeadChanged);
        }

        private void OnIsDeadChanged(bool arg1, bool isDead)
        {
            if (isDead)
            {
                _currentTime.Value = _initialTime.Value;
                _inDeathProcess.Value = true;
            }
        }

        public void OnUpdate(float deltaTime)
        {
            if(_inDeathProcess.Value == false)
                return;

            _currentTime.Value -= deltaTime;

            if(CooldownIsOver())
                _inDeathProcess.Value = false;
        }

        public void OnDispose() => _isDeadChangedDisposable.Dispose();

        private bool CooldownIsOver() => _currentTime.Value <= 0;
    }
}
