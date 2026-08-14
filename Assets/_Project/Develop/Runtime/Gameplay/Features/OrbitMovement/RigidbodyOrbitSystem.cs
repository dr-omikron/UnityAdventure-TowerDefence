using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.OrbitMovement
{
    public class RigidbodyOrbitSystem : IInitializableSystem, IUpdateableSystem
    {
        private ReactiveVariable<Vector3> _center;
        private ReactiveVariable<float> _radius;
        private ReactiveVariable<float> _angle;
        private ReactiveVariable<float> _angularSpeed;
        private Rigidbody _rigidbody;

        public void OnInit(Entity entity)
        {
            _center = entity.OrbitCenter;
            _radius = entity.OrbitRadius;
            _angle = entity.OrbitAngle;
            _angularSpeed = entity.OrbitAngularSpeed;
            _rigidbody = entity.Rigidbody;
        }

        public void OnUpdate(float deltaTime)
        {
            float angle = _angle.Value + _angularSpeed.Value * deltaTime;
            _angle.Value = Mathf.Repeat(angle, OrbitGeometry.FULL_CIRCLE_DEGREES);

            _rigidbody.MovePosition(OrbitGeometry.GetPointOn(_center.Value, _radius.Value, _angle.Value));
        }
    }
}
