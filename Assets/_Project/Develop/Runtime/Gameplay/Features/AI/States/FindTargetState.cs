using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Utilities.Reactive;
using _Project.Develop.Runtime.Utilities.StateMachineCore;

namespace _Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class FindTargetState : State, IUpdatableState
    {
        private readonly ITargetSelector _targetSelector;
        private readonly EntitiesLifeContext _entitiesLifeContext;
        private readonly ReactiveVariable<Entity> _currentTarget;

        public FindTargetState(Entity entity, ITargetSelector targetSelector, EntitiesLifeContext entitiesLifeContext)
        {
            _currentTarget = entity.CurrentTarget;
            _targetSelector = targetSelector;
            _entitiesLifeContext = entitiesLifeContext;
        }

        public void Update(float deltaTime)
        {
            _currentTarget.Value = _targetSelector.SelectTarget(_entitiesLifeContext.Entities);
        }
    }
}
