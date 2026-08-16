using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using _Project.Develop.Runtime.Utilities.Conditions;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.Attack.AreaAttack
{
    public class PeriodicAreaAttackSystem : IInitializableSystem, IUpdateableSystem
    {
        private ReactiveEvent<Vector3> _startAreaAttackRequest;
        private ICompositeCondition _canStartAttack;
        private Transform _transform;

        public void OnInit(Entity entity)
        {
            _startAreaAttackRequest = entity.StartAreaAttackRequest;
            _canStartAttack = entity.CanStartAttack;
            _transform = entity.Transform;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_canStartAttack.Evaluate() == false)
                return;

            _startAreaAttackRequest.Invoke(_transform.position);
        }
    }
}
