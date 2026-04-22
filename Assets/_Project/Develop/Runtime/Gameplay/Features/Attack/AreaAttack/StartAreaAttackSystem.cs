using System;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using _Project.Develop.Runtime.Utilities.Conditions;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.Attack.AreaAttack
{
    public class StartAreaAttackSystem : IInitializableSystem, IDisposableSystem
    {
        private ReactiveEvent<Vector3> _startAreaAttackRequest;
        private ReactiveEvent<Vector3> _castPositionEvent;
        private ReactiveEvent _startAttackEvent;

        private ReactiveVariable<bool> _inAttackProcess;
        private ICompositeCondition _canStartAttack;

        private IDisposable _startAreaAttackRequestDisposable;

        public void OnInit(Entity entity)
        {
            _startAreaAttackRequest = entity.StartAreaAttackRequest;
            _castPositionEvent = entity.CastAreaPositionEvent;
            _startAttackEvent = entity.StartAttackEvent;
            _inAttackProcess = entity.InAttackProcess;
            _canStartAttack = entity.CanStartAttack;
            _startAreaAttackRequestDisposable = _startAreaAttackRequest.Subscribe(OnStartAttackEvent);
        }

        private void OnStartAttackEvent(Vector3 position)
        {
            if(_canStartAttack.Evaluate())
            {
                _inAttackProcess.Value = true;
                _startAttackEvent.Invoke();
                _castPositionEvent.Invoke(position);
                Debug.Log("Start Area Attack");
            }
            else
            {
                Debug.Log("Can't Start Area Attack");
            }
        }

        public void OnDispose() => _startAreaAttackRequestDisposable.Dispose();
    }
}
