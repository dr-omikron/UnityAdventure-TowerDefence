using System;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.Attack.Shoot
{
    public class InstantShootSystem : IInitializableSystem, IDisposableSystem
    {
        private readonly EntitiesFactory _entitiesFactory;
        private Entity _entity;
        private ReactiveEvent _startAttackEvent;
        private ReactiveVariable<float> _damage;
        private Transform[] _shootPoints;

        private IDisposable _startAttackEventDisposable;

        public InstantShootSystem(EntitiesFactory entitiesFactory)
        {
            _entitiesFactory = entitiesFactory;
        }

        public void OnInit(Entity entity)
        {
            _startAttackEvent = entity.StartAttackEvent;
            _entity = entity;
            _damage = entity.ShootAttackDamage;
            _shootPoints = entity.ShootPoints;

            _startAttackEventDisposable = _startAttackEvent.Subscribe(OnStartAttackEvent);
        }

        private void OnStartAttackEvent()
        {
            foreach (var point in _shootPoints)
                _entitiesFactory.CreateProjectile(point.position, point.forward, _damage.Value, _entity);
        }

        public void OnDispose() => _startAttackEventDisposable.Dispose();
    }
}


