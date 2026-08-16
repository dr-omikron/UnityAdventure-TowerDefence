using System;
using System.Collections.Generic;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Infrastructure.DI;

namespace _Project.Develop.Runtime.Gameplay.Features.Shop
{
    public class SingleWaveEntitiesHolderService : IInitializable, IDisposable
    {
        private readonly EntitiesLifeContext _entitiesLifeContext;
        private readonly List<Entity> _entities = new List<Entity>();

        public SingleWaveEntitiesHolderService(EntitiesLifeContext entitiesLifeContext)
        {
            _entitiesLifeContext = entitiesLifeContext;
        }

        public IReadOnlyList<Entity> Entities => _entities;

        public void Initialize()
        {
            _entitiesLifeContext.Released += OnEntityReleased;
        }

        public void Add(Entity entity) => _entities.Add(entity);

        public void ReleaseAll()
        {
            foreach (Entity entity in _entities)
                _entitiesLifeContext.Release(entity);

            _entities.Clear();
        }

        public void Dispose()
        {
            _entitiesLifeContext.Released -= OnEntityReleased;
            _entities.Clear();
        }

        private void OnEntityReleased(Entity entity) => _entities.Remove(entity);
    }
}
