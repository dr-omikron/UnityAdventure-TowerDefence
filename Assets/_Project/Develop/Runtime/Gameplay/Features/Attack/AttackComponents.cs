using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Utilities.Conditions;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.Attack
{
    public class StartAttackRequest : IEntityComponent
    {
        public ReactiveEvent Value;
    }

    public class StartAreaAttackRequest : IEntityComponent
    {
        public ReactiveEvent<Vector3> Value;
    }

    public class StartAttackEvent : IEntityComponent
    {
        public ReactiveEvent Value;
    }

    public class EndAttackEvent : IEntityComponent
    {
        public ReactiveEvent Value;
    }
    
    public class CanStartAttack : IEntityComponent
    {
        public ICompositeCondition Value;
    }

    public class AttackProcessInitialTime : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class AttackProcessCurrentTime : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class InAttackProcess : IEntityComponent
    {
        public ReactiveVariable<bool> Value;
    }

    public class AttackDelayTime : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class AttackDelayEndEvent : IEntityComponent
    {
        public ReactiveEvent Value;
    }

    public class MustCanceledAttack : IEntityComponent
    {
        public ICompositeCondition Value;
    }

    public class AttackCanceledEvent : IEntityComponent
    {
        public ReactiveEvent Value;
    }

    public class AttackCooldownInitialTime : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class AttackCooldownCurrentTime : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class InAttackCooldown : IEntityComponent
    {
        public ReactiveVariable<bool> Value;
    }

    public class StartAttackDistance : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }
}
