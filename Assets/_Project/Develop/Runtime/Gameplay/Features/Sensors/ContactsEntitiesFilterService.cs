using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Utilities;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.Sensors
{
    public class ContactsEntitiesFilterService
    {
        private readonly ColliderRegistryService _colliderRegistryService;

        public ContactsEntitiesFilterService(ColliderRegistryService colliderRegistryService)
        {
            _colliderRegistryService = colliderRegistryService;
        }

        public void Process(Buffer<Collider> contactsColliders, Buffer<Entity> contactsEntities)
        {
            contactsEntities.Count = 0;

            for (int i = 0; i < contactsColliders.Count; i++)
            {
                Collider collider = contactsColliders.Items[i];
                Entity contactEntity = _colliderRegistryService.GetBy(collider);

                if (contactEntity != null)
                {
                    contactsEntities.Items[contactsEntities.Count] = contactEntity;
                    contactsEntities.Count++;
                }
            }
        }
    }
}
