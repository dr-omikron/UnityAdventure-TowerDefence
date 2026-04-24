using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Utilities.Reactive;
using _Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class MovementToRotationDirectionState : State, IUpdatableState
    {
        private readonly ReactiveVariable<Vector3> _rotationDirection;
        private readonly ReactiveVariable<Vector3> _movementDirection;

        public MovementToRotationDirectionState(Entity entity)
        {
            _rotationDirection = entity.RotationDirection;
            _movementDirection = entity.MoveDirection;
        }

        public override void Enter()
        {
            base.Enter();

            _movementDirection.Value = _rotationDirection.Value;
        }

        public override void Exit()
        {
            base.Exit();

            _movementDirection.Value = Vector3.zero;
        }

        public void Update(float deltaTime)
        {
        }
    }
}
