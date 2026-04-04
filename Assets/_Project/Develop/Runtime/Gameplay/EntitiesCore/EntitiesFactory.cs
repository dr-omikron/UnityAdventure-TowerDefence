using _Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using _Project.Develop.Runtime.Gameplay.Features.MovementFeatures;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.EntitiesCore
{
    public class EntitiesFactory
    {
        private readonly DIContainer _container;
        private readonly EntitiesLifeContext _entitiesLifeContext;
        private readonly MonoEntityFactory _monoEntityFactory;
        //private readonly ColliderRegistryService _colliderRegistryService;

        public EntitiesFactory(DIContainer container)
        {
            _container = container;
            _entitiesLifeContext = _container.Resolve<EntitiesLifeContext>();
            _monoEntityFactory = _container.Resolve<MonoEntityFactory>();
            //_colliderRegistryService = _container.Resolve<ColliderRegistryService>();
        }

        public Entity CreateSimpleEnemy(Vector3 position)
        {
            Entity entity = CreateEmpty();

            _monoEntityFactory.Create(entity, position, "Entities/SimpleEnemy");

            entity
                .AddMoveDirection(new ReactiveVariable<Vector3>())
                .AddMoveSpeed(new ReactiveVariable<float>(5))
                .AddRotationDirection()
                .AddRotationSpeed(new ReactiveVariable<float>(900));

            entity
                .AddSystem(new RigidbodyMovementSystem())
                .AddSystem(new RigidbodyRotationSystem());

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        private Entity CreateEmpty() => new Entity();
    }
}
