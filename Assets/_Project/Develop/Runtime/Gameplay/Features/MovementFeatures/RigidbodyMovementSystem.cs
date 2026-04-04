using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.MovementFeatures
{
    public class RigidbodyMovementSystem : IInitializableSystem, IUpdateableSystem
    {
        private ReactiveVariable<Vector3> _direction;
        private ReactiveVariable<float> _speed;
        private Rigidbody _rigidbody;

        public void OnInit(Entity entity)
        {
            _direction = entity.MoveDirection;
            _speed = entity.MoveSpeed;
            _rigidbody = entity.Rigidbody;
        }

        public void OnUpdate(float deltaTime)
        {
            Vector3 velocity = _direction.Value.normalized * _speed.Value;
            _rigidbody.velocity = velocity;
        }
    }
}
