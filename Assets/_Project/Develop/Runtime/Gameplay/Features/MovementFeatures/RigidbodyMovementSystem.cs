using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using _Project.Develop.Runtime.Utilities.Conditions;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.MovementFeatures
{
    public class RigidbodyMovementSystem : IInitializableSystem, IUpdateableSystem
    {
        private ReactiveVariable<Vector3> _direction;
        private ReactiveVariable<float> _speed;
        private ReactiveVariable<bool> _isMoving;
        private Rigidbody _rigidbody;
        private ICompositeCondition _canMove;

        public void OnInit(Entity entity)
        {
            _direction = entity.MoveDirection;
            _speed = entity.MoveSpeed;
            _isMoving = entity.IsMoving;
            _rigidbody = entity.Rigidbody;
            _canMove = entity.CanMove;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_canMove.Evaluate() == false)
            {
                _rigidbody.velocity = Vector3.zero;
                return;
            }

            Vector3 velocity = _direction.Value.normalized * _speed.Value;
            _isMoving.Value = velocity.magnitude > 0;
            _rigidbody.velocity = velocity;
        }
    }
}
