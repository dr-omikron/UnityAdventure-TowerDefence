using UnityEngine;

namespace _Project.Develop.Runtime.Configs.Gameplay.Entities
{
    [CreateAssetMenu(menuName = "Configs/Gameplay/Entities/ShootingEnemyConfig", fileName = "ShootingEnemyConfig")]

    public class ShootingEnemyConfig : EntityConfig
    {
        [field: SerializeField] public string PrefabPath { get; private set; } = "Entities/ShootingEnemy";
        [field: SerializeField, Min(0)] public float MoveSpeed { get; private set; } = 25;
        [field: SerializeField, Min(0)] public float RotationSpeed { get; private set; } = 900;
        [field: SerializeField, Min(0)] public float MaxHealth { get; private set; } = 100;
        [field: SerializeField, Min(0)] public float DeathProcessTime { get; private set; } = 0.1f;
        [field: SerializeField, Min(0)] public float AttackProcessInitialTime { get; private set; } = 1;
        [field: SerializeField, Min(0)] public float AttackCooldown { get; private set; } = 2;
        [field: SerializeField, Min(0)] public float AttackDamage { get; private set; } = 25;
        [field: SerializeField, Min(0)] public float StartAttackDistance { get; private set; } = 50;
    }
}
