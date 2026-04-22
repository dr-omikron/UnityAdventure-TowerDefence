using System;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Utilities.Reactive;

namespace _Project.Develop.Runtime.Gameplay.Features.Station
{
    public class StationHolderService : IInitializable, IDisposable
    {
        private readonly EntitiesLifeContext _entitiesLifeContext;
        private ReactiveEvent<Entity> _stationRegistered;
        private Entity _station;

        public StationHolderService(EntitiesLifeContext entitiesLifeContext)
        {
            _entitiesLifeContext = entitiesLifeContext;
        }

        public IReadOnlyEvent<Entity> StationRegistered => _stationRegistered;
        public Entity Station => _station;

        public void Initialize()
        {
            _entitiesLifeContext.Added += OnEntityAdded;
        }

        private void OnEntityAdded(Entity entity)
        {
            if (entity.HasComponent<IsStation>())
            {
                _entitiesLifeContext.Added -= OnEntityAdded;
                _station = entity;
                _stationRegistered?.Invoke(_station);
            }
        }

        public void Dispose()
        {
            _entitiesLifeContext.Added -= OnEntityAdded;
        }
    }
}
