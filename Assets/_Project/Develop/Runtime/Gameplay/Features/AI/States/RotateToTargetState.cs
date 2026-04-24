using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Utilities.Reactive;
using _Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class RotateToTargetState : State, IUpdatableState
    {
        private readonly ReactiveVariable<Entity> _target;
        private readonly Transform _transform;
        private readonly ReactiveVariable<Vector3> _rotationDirection;

        public RotateToTargetState(Entity entity)
        {
            _target = entity.CurrentTarget;
            _transform = entity.Transform;
            _rotationDirection = entity.RotationDirection;
        }

        public void Update(float deltaTime)
        {
            if(_target.Value != null && _target.Value.Transform != null)
                _rotationDirection.Value = (_target.Value.Transform.position - _transform.position).normalized;
        }
    }
}
