using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using _Project.Develop.Runtime.Utilities.Conditions;

namespace _Project.Develop.Runtime.Gameplay.Features.LifeCycle
{
    public class SelfReleaseSystem : IInitializableSystem, IUpdateableSystem
    {
        private readonly EntitiesLifeContext _entitiesLifeContext;
        private Entity _entity;
        private ICompositeCondition _mustSelfRelease;

        public SelfReleaseSystem(EntitiesLifeContext entitiesLifeContext)
        {
            _entitiesLifeContext = entitiesLifeContext;
        }

        public void OnInit(Entity entity)
        {
            _entity = entity;
            _mustSelfRelease = entity.MustSelfRelease;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_mustSelfRelease.Evaluate())
                _entitiesLifeContext.Release(_entity);
        }
    }
}
