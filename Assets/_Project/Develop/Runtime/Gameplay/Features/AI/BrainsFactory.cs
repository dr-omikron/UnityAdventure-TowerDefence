using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.Features.AI.States;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Utilities.Conditions;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.AI
{
    public class BrainsFactory
    {
        private readonly DIContainer _container;
        private readonly AIBrainContext _brainContext;
        private readonly EntitiesLifeContext _entitiesLifeContext;

        public BrainsFactory(DIContainer container)
        {
            _container = container;
            _brainContext = _container.Resolve<AIBrainContext>();
            _entitiesLifeContext = _container.Resolve<EntitiesLifeContext>();
        }

        public StateMachineBrain CreateSimpleEnemyBrain(Entity entity)
        {
            AIStateMachine stateMachine = CreateMovementToTargetStateMachine(entity);
            StateMachineBrain brain = new StateMachineBrain(stateMachine);
            _brainContext.SetFor(entity, brain);
            return brain;
        }

        public StateMachineBrain CreateTurretBrain(Entity entity, ITargetSelector targetSelector)
        {
            FindTargetState findTargetState = new FindTargetState(entity, targetSelector, _entitiesLifeContext);
            AIStateMachine combatState = CreateAutoAttackStateMachine(entity);

            AIParallelState parallelState = new AIParallelState(findTargetState, combatState);

            AIStateMachine rootStateMachine = new AIStateMachine();
            rootStateMachine.AddState(parallelState);

            StateMachineBrain brain = new StateMachineBrain(rootStateMachine);
            _brainContext.SetFor(entity, brain);

            return brain;
        }

        public StateMachineBrain CreateShootingEnemyBrain(Entity entity)
        {
            AIStateMachine movementState = CreateMovementToTargetStateMachine(entity);
            AIStateMachine combatState = CreateAutoAttackStateMachine(entity);

            ReactiveVariable<Entity> target = entity.CurrentTarget;
            Transform transform = entity.Transform;
            ReactiveVariable<float> startAttackDistance = entity.StartAttackDistance;

            ICondition fromMovementToCombatState = new FuncCondition(() =>
            {
                if (target.Value == null)
                    return false;

                float distance = (transform.position - target.Value.Transform.position).magnitude;
                return distance <= startAttackDistance.Value;
            });

            AIStateMachine behaviour = new AIStateMachine();

            behaviour.AddState(movementState);
            behaviour.AddState(combatState);

            behaviour.AddTransition(movementState, combatState, fromMovementToCombatState);

            StateMachineBrain brain = new StateMachineBrain(behaviour);
            _brainContext.SetFor(entity, brain);

            return brain;
        }

        private AIStateMachine CreateMovementToTargetStateMachine(Entity entity)
        {
            RotateToTargetState rotateToTargetState = new RotateToTargetState(entity);
            MovementToRotationDirectionState movementToRotationDirectionState = new MovementToRotationDirectionState(entity);
            EmptyState emptyState = new EmptyState();

            ReactiveVariable<Entity> target = entity.CurrentTarget;
            Transform transform = entity.Transform;

            ICondition fromRotateToMovementState = new FuncCondition(() =>
            {
                if (target.Value == null)
                    return false;

                float angleToTarget = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(target.Value.Transform.position - transform.position));

                return angleToTarget <= 3f;
            });

            ICondition fromMovementToEmptyState = new FuncCondition(() => target == null);

            AIStateMachine stateMachine = new AIStateMachine();

            stateMachine.AddState(rotateToTargetState);
            stateMachine.AddState(movementToRotationDirectionState);
            stateMachine.AddState(emptyState);

            stateMachine.AddTransition(rotateToTargetState, movementToRotationDirectionState, fromRotateToMovementState);
            stateMachine.AddTransition(movementToRotationDirectionState, emptyState, fromMovementToEmptyState);

            return stateMachine;
        }

        private AIStateMachine CreateAutoAttackStateMachine(Entity entity)
        {
            RotateToTargetState rotateToTargetState = new RotateToTargetState(entity);
            AttackTriggerState attackTriggerState = new AttackTriggerState(entity);

            ReactiveVariable<Entity> target = entity.CurrentTarget;
            Transform transform = entity.Transform;

            ICondition canAttack = entity.CanStartAttack;

            ICompositeCondition fromRotateToAttackCondition = new CompositeCondition()
                .Add(canAttack)
                .Add(new FuncCondition(() =>
                {
                    if (target.Value == null)
                        return false;

                    float angleToTarget = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(target.Value.Transform.position - transform.position));

                    return angleToTarget <= 3f;
                }));

            ReactiveVariable<bool> inAttackProcess = entity.InAttackProcess;
            ICondition fromAttackToRotateCondition = new FuncCondition(() => inAttackProcess.Value == false);

            AIStateMachine stateMachine = new AIStateMachine();

            stateMachine.AddState(rotateToTargetState);
            stateMachine.AddState(attackTriggerState);

            stateMachine.AddTransition(rotateToTargetState, attackTriggerState, fromRotateToAttackCondition);
            stateMachine.AddTransition(attackTriggerState, rotateToTargetState, fromAttackToRotateCondition);

            return stateMachine;
        }
    }
}
