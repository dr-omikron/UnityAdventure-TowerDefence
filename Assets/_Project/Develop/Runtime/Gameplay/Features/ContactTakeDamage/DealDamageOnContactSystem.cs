using System.Collections.Generic;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using _Project.Develop.Runtime.Gameplay.Features.ApplyDamage;
using _Project.Develop.Runtime.Utilities;
using _Project.Develop.Runtime.Utilities.Reactive;

namespace _Project.Develop.Runtime.Gameplay.Features.ContactTakeDamage
{
    public class DealDamageOnContactSystem : IInitializableSystem, IUpdateableSystem
    {
        private Buffer<Entity> _contacts;
        private ReactiveVariable<float> _damage;
        private List<Entity> _processedEntities;

        public void OnInit(Entity entity)
        {
            _contacts = entity.ContactEntitiesBuffer;
            _damage = entity.BodyContactDamage;

            _processedEntities = new List<Entity>(_contacts.Items.Length);
        }

        public void OnUpdate(float deltaTime)
        {
            for (int i = 0; i < _contacts.Count; i++)
            {
                Entity contactEntity = _contacts.Items[i];

                if (_processedEntities.Contains(contactEntity) == false)
                {
                    _processedEntities.Add(contactEntity);

                    if (contactEntity.HasComponent<TakeDamageRequest>())
                        contactEntity.TakeDamageRequest.Invoke(_damage.Value);
                }
            }

            for (int i = _processedEntities.Count - 1; i >= 0; i--)
            {
                if(ContainInContacts(_processedEntities[i]) == false)
                    _processedEntities.RemoveAt(i);
            }
        }

        private bool ContainInContacts(Entity entity)
        {
            for (int i = 0; i < _contacts.Count; i++)
            {
                if(_contacts.Items[i] == entity)
                    return true;
            }

            return false;
        }
    }
}
