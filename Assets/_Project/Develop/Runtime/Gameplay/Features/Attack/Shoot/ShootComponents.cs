using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.Attack.Shoot
{
    public class ShootAttackDamage : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class ShootPoint : IEntityComponent
    {
        public Transform Value;
    }
}
