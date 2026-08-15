using UnityEngine;

namespace _Project.Develop.Runtime.Configs.Gameplay.Abilities
{
    [CreateAssetMenu(menuName = "Configs/Gameplay/Abilities/NewExplosionAbilityConfig", fileName = "ExplosionAbilityConfig")]
    public class ExplosionAbilityConfig : ScriptableObject
    {
        [field: SerializeField, Min(0)] public float Damage { get; private set; } = 50;
        [field: SerializeField, Min(0)] public float Radius { get; private set; } = 25;
        [field: SerializeField, Min(0)] public float Cooldown { get; private set; } = 3;
        [field: SerializeField, Min(0)] public float CastTime { get; private set; } = 0.1f;
    }
}
