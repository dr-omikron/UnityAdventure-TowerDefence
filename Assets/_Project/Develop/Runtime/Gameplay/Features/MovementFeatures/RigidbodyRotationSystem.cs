using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using _Project.Develop.Runtime.Utilities.Conditions;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.MovementFeatures
{
    public class RigidbodyRotationSystem : IInitializableSystem, IUpdateableSystem
    {
        private Rigidbody _rigidbody;
        private ReactiveVariable<Vector3> _direction;
        private ReactiveVariable<float> _speed;
        private ICompositeCondition _canRotate;

        public void OnInit(Entity entity)
        {
            _rigidbody = entity.Rigidbody;
            _direction = entity.RotationDirection;
            _speed = entity.RotationSpeed;
            _canRotate = entity.CanRotate;

            if(_direction.Value != Vector3.zero)
                _rigidbody.transform.rotation = Quaternion.LookRotation(_direction.Value.normalized);
        }

        public void OnUpdate(float deltaTime)
        {
            if (_canRotate.Evaluate() == false)
                return;

            if(_direction.Value == Vector3.zero)
                return;

            Quaternion lookRotation = Quaternion.LookRotation(_direction.Value.normalized);
            float step = _speed.Value * deltaTime;
            Quaternion rotation = Quaternion.RotateTowards(_rigidbody.rotation, lookRotation, step);
            _rigidbody.MoveRotation(rotation);
        }
    }
}
