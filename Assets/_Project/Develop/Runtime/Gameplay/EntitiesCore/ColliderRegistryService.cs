using System.Collections.Generic;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.EntitiesCore
{
    public class ColliderRegistryService
    {
        private readonly Dictionary<Collider, Entity> _colliderToEntity = new Dictionary<Collider, Entity>();

        public void Register(Collider collider, Entity entity)
        {
            _colliderToEntity.Add(collider, entity);
        }

        public void Unregister(Collider collider)
        {
            _colliderToEntity.Remove(collider);
        }

        public Entity GetBy(Collider collider)
        {
            return _colliderToEntity.GetValueOrDefault(collider);
        }
    }
}
