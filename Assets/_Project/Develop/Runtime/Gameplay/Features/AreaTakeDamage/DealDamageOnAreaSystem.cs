using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using _Project.Develop.Runtime.Utilities;
using _Project.Develop.Runtime.Utilities.Reactive;

namespace _Project.Develop.Runtime.Gameplay.Features.AreaTakeDamage
{
    public class DealDamageOnAreaSystem : IInitializableSystem, IUpdateableSystem
    {
        private Entity _source;
        private Buffer<Entity> _contacts;
        private ReactiveVariable<float> _damage;

        public void OnInit(Entity entity)
        {
            _source = entity;
            _contacts = entity.AreaEntitiesBuffer;
            _damage = entity.AreaDamage;
        }

        public void OnUpdate(float deltaTime)
        {
            if(_contacts.Count == 0)
                return;

            for (int i = 0; i < _contacts.Count; i++)
            {
                Entity contactEntity = _contacts.Items[i];
                EntitiesHelper.TryTakeDamageFor(_source, contactEntity, _damage.Value);
            }

            _contacts.Count = 0;
        }
    }
}
