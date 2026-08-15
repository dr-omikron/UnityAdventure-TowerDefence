using System;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.Sensors
{
    public class AreaCastDebugDrawSystem : IInitializableSystem, IDisposableSystem
    {
        private const int SEGMENTS_COUNT = 32;
        private const float DRAW_DURATION = 1f;
        private const float FULL_CIRCLE_RADIANS = Mathf.PI * 2;

        private readonly Color _color;

        private ReactiveEvent<Vector3> _castPositionEvent;
        private ReactiveVariable<float> _radius;

        private IDisposable _castPositionDisposable;

        public AreaCastDebugDrawSystem(Color color)
        {
            _color = color;
        }

        public void OnInit(Entity entity)
        {
            _castPositionEvent = entity.CastAreaPositionEvent;
            _radius = entity.AreaDetectingRadius;

            _castPositionDisposable = _castPositionEvent.Subscribe(OnCastPositionEvent);
        }

        public void OnDispose() => _castPositionDisposable.Dispose();

        private void OnCastPositionEvent(Vector3 position)
        {
            float radius = _radius.Value;

            DrawCircle(position, radius, Vector3.right, Vector3.forward);
            DrawCircle(position, radius, Vector3.right, Vector3.up);
            DrawCircle(position, radius, Vector3.forward, Vector3.up);
        }

        private void DrawCircle(Vector3 center, float radius, Vector3 firstAxis, Vector3 secondAxis)
        {
            float angleStep = FULL_CIRCLE_RADIANS / SEGMENTS_COUNT;
            Vector3 previousPoint = center + firstAxis * radius;

            for (int i = 1; i <= SEGMENTS_COUNT; i++)
            {
                float angle = angleStep * i;
                Vector3 offset = firstAxis * (Mathf.Cos(angle) * radius) + secondAxis * (Mathf.Sin(angle) * radius);
                Vector3 point = center + offset;

                Debug.DrawLine(previousPoint, point, _color, DRAW_DURATION);
                previousPoint = point;
            }
        }
    }
}
