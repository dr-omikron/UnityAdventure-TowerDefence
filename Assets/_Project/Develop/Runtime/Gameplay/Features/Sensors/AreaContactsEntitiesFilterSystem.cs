using System;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using _Project.Develop.Runtime.Utilities;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.Sensors
{
    public class AreaContactsEntitiesFilterSystem : IInitializableSystem, IDisposableSystem
    {
        private Buffer<Entity> _contactsEntities;
        private Buffer<Collider> _contacts;
        private ReactiveEvent _endCastAreaPositionEvent;

        private IDisposable _onEndCastEventDisposable;

        private readonly ContactsEntitiesFilterService _contactsEntitiesFilterService;

        public AreaContactsEntitiesFilterSystem(ContactsEntitiesFilterService contactsEntitiesFilterService)
        {
            _contactsEntitiesFilterService = contactsEntitiesFilterService;
        }

        public void OnInit(Entity entity)
        {
            _contacts = entity.AreaCollidersBuffer;
            _contactsEntities = entity.AreaEntitiesBuffer;
            _endCastAreaPositionEvent = entity.EndCastAreaPositionEvent;

            _onEndCastEventDisposable = _endCastAreaPositionEvent.Subscribe(OnEndCastAreaPositionEvent);
        }

        private void OnEndCastAreaPositionEvent()
        {
            _contactsEntitiesFilterService.Process(_contacts, _contactsEntities);
        }

        public void OnDispose() => _onEndCastEventDisposable.Dispose();
    }
}
