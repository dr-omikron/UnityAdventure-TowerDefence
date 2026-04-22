using System;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using _Project.Develop.Runtime.Utilities.Reactive;

namespace _Project.Develop.Runtime.Gameplay.Features.Attack
{
    public class AttackProcessTimerSystem : IInitializableSystem, IUpdateableSystem, IDisposableSystem
    {
        private ReactiveVariable<float> _currentTime;
        private ReactiveVariable<bool> _inAttackProcess;
        private ReactiveEvent _startAttackEvent;

        private IDisposable _startAttackEventDisposable;

        public void OnInit(Entity entity)
        {
            _currentTime = entity.AttackProcessCurrentTime;
            _inAttackProcess = entity.InAttackProcess;
            _startAttackEvent = entity.StartAttackEvent;

            _startAttackEventDisposable = _startAttackEvent.Subscribe(OnStartAttackProcess);
        }

        private void OnStartAttackProcess()
        {
            _currentTime.Value = 0;
        }

        public void OnUpdate(float deltaTime)
        {
            if(_inAttackProcess.Value == false)
                return;

            _currentTime.Value += deltaTime;
        }

        public void OnDispose() => _startAttackEventDisposable.Dispose();
    }
}
