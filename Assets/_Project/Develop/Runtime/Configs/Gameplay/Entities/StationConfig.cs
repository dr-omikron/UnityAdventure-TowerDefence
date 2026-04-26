using UnityEngine;

namespace _Project.Develop.Runtime.Configs.Gameplay.Entities
{
    [CreateAssetMenu(menuName = "Configs/Gameplay/Entities/StationConfig", fileName = "StationConfig")]
    public class StationConfig : EntityConfig
    {
        [field: SerializeField] public string PrefabPath { get; private set; } = "Entities/Station";
        [field: SerializeField, Min(0)] public float AreaDamage { get; private set; } = 50;
        [field: SerializeField, Min(0)] public float AreaDamageRadius { get; private set; } = 30;
        [field: SerializeField, Min(0)] public float AttackProcessInitialTime { get; private set; } = 1;
        [field: SerializeField, Min(0)] public float AttackCooldown { get; private set; } = 2;
        [field: SerializeField, Min(0)] public float DeathProcessTime { get; private set; } = 2;
    }
}
