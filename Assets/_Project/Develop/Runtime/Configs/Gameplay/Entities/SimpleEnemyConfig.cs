using UnityEngine;

namespace _Project.Develop.Runtime.Configs.Gameplay.Entities
{
    [CreateAssetMenu(menuName = "Configs/Gameplay/Entities/SimpleEnemyConfig", fileName = "SimpleEnemyConfig")]

    public class SimpleEnemyConfig : EntityConfig
    {
        [field: SerializeField] public string PrefabPath { get; private set; } = "Entities/SimpleEnemy";
        [field: SerializeField, Min(0)] public float MoveSpeed { get; private set; } = 25;
        [field: SerializeField, Min(0)] public float RotationSpeed { get; private set; } = 900;
        [field: SerializeField, Min(0)] public float MaxHealth { get; private set; } = 100;
        [field: SerializeField, Min(0)] public float BodyContactDamage { get; private set; } = 25;
        [field: SerializeField, Min(0)] public float DeathProcessTime { get; private set; } = 0.1f;
    }
}
