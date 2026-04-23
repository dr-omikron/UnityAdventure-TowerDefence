using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using _Project.Develop.Runtime.Utilities.Conditions;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.Attack
{
    public class AttackCanceledSystem : IInitializableSystem, IUpdateableSystem
    {
        private ReactiveVariable<bool> _inAttackProcess;
        private ReactiveEvent _attackCanceledEvent;
        private ICompositeCondition _mustCanceledAttack;

        public void OnInit(Entity entity)
        {
            _inAttackProcess = entity.InAttackProcess;
            _attackCanceledEvent = entity.AttackCanceledEvent;
            _mustCanceledAttack = entity.MustCanceledAttack;
        }

        public void OnUpdate(float deltaTime)
        {
            if(_inAttackProcess.Value == false)
                return;

            if (_mustCanceledAttack.Evaluate())
            {
                Debug.Log("Attack Canceled");
                _inAttackProcess.Value = false;
                _attackCanceledEvent.Invoke();
            }
        }
    }
}
