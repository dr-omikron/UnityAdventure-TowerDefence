using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Utilities.Reactive;
using _Project.Develop.Runtime.Utilities.StateMachineCore;

namespace _Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class AttackTriggerState : State, IUpdatableState
    {
        private readonly ReactiveEvent _attackRequest;

        public AttackTriggerState(Entity entity)
        {
            _attackRequest = entity.StartAttackRequest;
        }

        public override void Enter()
        {
            base.Enter();
            _attackRequest.Invoke();
        }

        public void Update(float deltaTime)
        {
        }
    }
}
