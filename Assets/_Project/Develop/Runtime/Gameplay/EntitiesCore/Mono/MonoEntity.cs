using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.EntitiesCore.Mono
{
    public class MonoEntity : MonoBehaviour
    {
        private ColliderRegistryService _colliderRegistryService;
        private Entity _linkedEntity;

        public Entity LinkedEntity => _linkedEntity;

        public void Initialize(ColliderRegistryService colliderRegistryService)
        {
            _colliderRegistryService = colliderRegistryService;
        }

        public void Link(Entity entity)
        {
            _linkedEntity = entity;

            MonoEntityRegistrator[] registrators = GetComponentsInChildren<MonoEntityRegistrator>();

            if (registrators != null)
                foreach (MonoEntityRegistrator registrator in registrators)
                    registrator.Register(entity);

            EntityView[] views = GetComponentsInChildren<EntityView>();

            if (views != null)
                foreach (EntityView view in views)
                    view.Link(entity);

            foreach (Collider collider in GetComponentsInChildren<Collider>())
                _colliderRegistryService.Register(collider, entity);
        }

        public void Cleanup(Entity entity)
        {
            EntityView[] views = GetComponentsInChildren<EntityView>();

            if (views != null)
                foreach (EntityView view in views)
                    view.Cleanup(entity);

            foreach (Collider collider in GetComponentsInChildren<Collider>())
                _colliderRegistryService.Unregister(collider);

            _linkedEntity = null;
        }
    }
}
