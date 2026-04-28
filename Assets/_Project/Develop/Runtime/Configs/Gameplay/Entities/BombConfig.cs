using UnityEngine;

namespace _Project.Develop.Runtime.Configs.Gameplay.Entities
{
    [CreateAssetMenu(menuName = "Configs/Gameplay/Entities/BombConfig", fileName = "BombConfig")]

    public class BombConfig : EntityConfig
    {
        [field: SerializeField] public string PrefabPath { get; private set; } = "Entities/Bomb";
        [field: SerializeField, Min(0)] public float BodyContactDamage { get; private set; } = 25;
    }
}
