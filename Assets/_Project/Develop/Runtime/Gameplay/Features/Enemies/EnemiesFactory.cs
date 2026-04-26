using System;
using _Project.Develop.Runtime.Configs.Gameplay.Entities;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.Features.AI;
using _Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Utilities.Reactive;
using Vector3 = UnityEngine.Vector3;

namespace _Project.Develop.Runtime.Gameplay.Features.Enemies
{
    public class EnemiesFactory
    {
        private readonly DIContainer _container;

        private readonly EntitiesFactory _entitiesFactory;
        private readonly BrainsFactory _brainsFactory;
        private readonly EntitiesLifeContext _entitiesLifeContext;

        public EnemiesFactory(DIContainer container)
        {
            _container = container;
            _brainsFactory = container.Resolve<BrainsFactory>();
            _entitiesFactory = container.Resolve<EntitiesFactory>();
            _entitiesLifeContext = container.Resolve<EntitiesLifeContext>();
        }

        public Entity Create(Vector3 position, EntityConfig entityConfig, Entity enemyTarget)
        {
            Entity entity;

            switch (entityConfig)
            {
                case SimpleEnemyConfig simpleEnemyConfig:
                    entity = _entitiesFactory.CreateSimpleEnemy(position, new ReactiveVariable<Entity>(enemyTarget), simpleEnemyConfig);
                    _brainsFactory.CreateSimpleEnemyBrain(entity);
                    break;

                case ShootingEnemyConfig shootingEnemyConfig:
                    entity = _entitiesFactory.CreateShootingEnemy(position, new ReactiveVariable<Entity>(enemyTarget), shootingEnemyConfig);
                    _brainsFactory.CreateShootingEnemyBrain(entity);
                    break;

                default:
                    throw new ArgumentException($"Not supported type config {entityConfig.GetType()}");
            }

            entity
                .AddTeam(new ReactiveVariable<Teams>(Teams.Enemies));

            _entitiesLifeContext.Add(entity);

            return entity;
        }
    }
}
