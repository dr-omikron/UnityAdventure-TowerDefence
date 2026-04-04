using _Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using _Project.Develop.Runtime.Infrastructure.DI;

namespace _Project.Develop.Runtime.Gameplay.EntitiesCore
{
    public class EntitiesFactory
    {
        private readonly DIContainer _container;
        private readonly EntitiesLifeContext _entitiesLifeContext;
        private readonly ColliderRegistryService _colliderRegistryService;
        private readonly MonoEntityFactory _monoEntityFactory;

        public EntitiesFactory(DIContainer container)
        {
            _container = container;
            _entitiesLifeContext = _container.Resolve<EntitiesLifeContext>();
            _colliderRegistryService = _container.Resolve<ColliderRegistryService>();
            _monoEntityFactory = _container.Resolve<MonoEntityFactory>();
        }

        private Entity CreateEmpty() => new Entity();
    }
}
