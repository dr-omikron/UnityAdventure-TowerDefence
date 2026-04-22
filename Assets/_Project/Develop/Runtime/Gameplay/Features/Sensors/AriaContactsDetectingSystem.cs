using System;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using _Project.Develop.Runtime.Utilities;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.Sensors
{
    public class AriaContactsDetectingSystem : IInitializableSystem, IDisposableSystem
    {
        private Buffer<Collider> _contacts;
        private LayerMask _layerMask;
        private ReactiveVariable<float> _radius;
        private ReactiveEvent<Vector3> _castPositionEvent;
        private ReactiveEvent _endCastPositionEvent;

        private IDisposable _onPositionEventDisposable;


        public void OnInit(Entity entity)
        {
            _contacts = entity.AreaCollidersBuffer;
            _layerMask = entity.AreaDetectingMask;
            _radius = entity.AreaDetectingRadius;
            _castPositionEvent = entity.CastAreaPositionEvent;
            _endCastPositionEvent = entity.EndCastAreaPositionEvent;

            _onPositionEventDisposable = _castPositionEvent.Subscribe(OnCastPositionEvent);
        }

        private void OnCastPositionEvent(Vector3 position)
        {
            _contacts.Count = Physics.OverlapSphereNonAlloc(
                position,
                _radius.Value,
                _contacts.Items,
                _layerMask,
                QueryTriggerInteraction.Ignore);

            _endCastPositionEvent.Invoke();

            Debug.Log($"{_contacts.Count} contacts detected {position} ");
        }

        public void OnDispose() => _onPositionEventDisposable.Dispose();
    }
}
