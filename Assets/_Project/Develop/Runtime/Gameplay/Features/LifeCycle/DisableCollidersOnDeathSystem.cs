using System;
using System.Collections.Generic;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.LifeCycle
{
    public class DisableCollidersOnDeathSystem : IInitializableSystem, IDisposableSystem
    {
        private List<Collider> _colliders;
        private ReactiveVariable<bool> _isDead;

        IDisposable _isDeadChangedDisposable;

        public void OnInit(Entity entity)
        {
            _colliders = entity.DisableCollidersOnDeath;
            _isDead = entity.IsDead;

            _isDeadChangedDisposable = _isDead.Subscribe(OnIsDeadChanged);
        }

        private void OnIsDeadChanged(bool arg1, bool isDead)
        {
            if (isDead)
                foreach(Collider collider in _colliders)
                    collider.enabled = false;
        }

        public void OnDispose() => _isDeadChangedDisposable.Dispose();
    }
}
