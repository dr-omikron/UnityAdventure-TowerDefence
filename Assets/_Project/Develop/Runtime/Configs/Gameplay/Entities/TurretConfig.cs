using UnityEngine;

namespace _Project.Develop.Runtime.Configs.Gameplay.Entities
{
    [CreateAssetMenu(menuName = "Configs/Gameplay/Entities/TurretConfig", fileName = "TurretConfig")]

    public class TurretConfig : EntityConfig
    {
        [field: SerializeField] public string PrefabPath { get; private set; } = "Entities/Turret";

        [field: SerializeField, Min(0)] public float RotationSpeed { get; private set; } = 900;
        [field: SerializeField, Min(0)] public float AttackProcessInitialTime { get; private set; } = 1;
        [field: SerializeField, Min(0)] public float AttackCooldown { get; private set; } = 2;
        [field: SerializeField, Min(0)] public float AttackDamage { get; private set; } = 25;

        [field: SerializeField, Min(0)] public float OrbitRadius { get; private set; } = 25;
        [field: SerializeField, Min(0)] public float OrbitHeight { get; private set; } = 30;
        [field: SerializeField, Min(0)] public float OrbitAngularSpeed { get; private set; } = 30;
    }
}
