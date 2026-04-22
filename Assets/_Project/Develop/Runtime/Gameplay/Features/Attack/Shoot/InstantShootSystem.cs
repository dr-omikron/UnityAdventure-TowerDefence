using System;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.Attack.Shoot
{
    public class InstantShootSystem : IInitializableSystem, IDisposableSystem
    {
        private ReactiveEvent _startAttackEvent;
        private ReactiveVariable<float> _damage;
        private Transform _shootPoint;

        private IDisposable _startAttackEventDisposable;

        public void OnInit(Entity entity)
        {
            _startAttackEvent = entity.StartAttackEvent;
            _damage = entity.ShootAttackDamage;
            _shootPoint = entity.ShootPoint;

            _startAttackEventDisposable = _startAttackEvent.Subscribe(OnStartAttackEvent);
        }

        private void OnStartAttackEvent()
        {
            Debug.Log($"Shoot - damage: {_damage.Value}");
        }

        public void OnDispose()
        {
            _startAttackEventDisposable.Dispose();
        }
    }
}
