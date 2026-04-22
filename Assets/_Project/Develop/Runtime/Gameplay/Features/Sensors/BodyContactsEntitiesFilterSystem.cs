using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using _Project.Develop.Runtime.Utilities;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.Sensors
{
    public class BodyContactsEntitiesFilterSystem : IInitializableSystem, IUpdateableSystem
    {
        private Buffer<Entity> _contactsEntities;
        private Buffer<Collider> _contacts;

        private readonly ContactsEntitiesFilterService _contactsEntitiesFilterService;

        public BodyContactsEntitiesFilterSystem(ContactsEntitiesFilterService contactsEntitiesFilterService)
        {
            _contactsEntitiesFilterService = contactsEntitiesFilterService;
        }

        public void OnInit(Entity entity)
        {
            _contacts = entity.ContactCollidersBuffer;
            _contactsEntities = entity.ContactEntitiesBuffer;
        }

        public void OnUpdate(float deltaTime)
        {
            _contactsEntitiesFilterService.Process(_contacts, _contactsEntities);
        }
    }
}
