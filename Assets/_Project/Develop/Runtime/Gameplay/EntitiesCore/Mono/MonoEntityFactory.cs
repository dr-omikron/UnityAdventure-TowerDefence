using System;
using System.Collections.Generic;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Utilities.AssetsManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.Develop.Runtime.Gameplay.EntitiesCore.Mono
{
    public class MonoEntityFactory : IInitializable, IDisposable
    {
        private readonly ResourcesAssetsLoader _resourcesAssetsLoader;
        private readonly EntitiesLifeContext _entitiesLifeContext;
        private readonly ColliderRegistryService _colliderRegistryService;

        private readonly Dictionary<Entity, MonoEntity> _entityToMono = new Dictionary<Entity, MonoEntity>();

        public MonoEntityFactory(
            ResourcesAssetsLoader resourcesAssetsLoader, 
            EntitiesLifeContext entitiesLifeContext, 
            ColliderRegistryService colliderRegistryService)
        {
            _resourcesAssetsLoader = resourcesAssetsLoader;
            _entitiesLifeContext = entitiesLifeContext;
            _colliderRegistryService = colliderRegistryService;
        }

        public MonoEntity Create(Entity entity, Vector3 position, string path)
        {
            MonoEntity prefab = _resourcesAssetsLoader.Load<MonoEntity>(path);
            MonoEntity instance = Object.Instantiate(prefab, position, Quaternion.identity, null);

            instance.Initialize(_colliderRegistryService);
            instance.Link(entity);

            _entityToMono.Add(entity, instance);
            return instance;
        }

        public void Initialize()
        {
            _entitiesLifeContext.Released += OnEntityReleased;
        }

        public void Dispose()
        {
            _entitiesLifeContext.Released -= OnEntityReleased;

            foreach (Entity entity in _entityToMono.Keys)
                CleanupFor(entity);

            _entityToMono.Clear();
        }

        private void OnEntityReleased(Entity entity)
        {
            CleanupFor(entity);

            _entityToMono.Remove(entity);
        }

        private void CleanupFor(Entity entity)
        {
            MonoEntity monoEntity = _entityToMono[entity];
            monoEntity.Cleanup(entity);
            Object.Destroy(monoEntity.gameObject);
        }
    }
}
