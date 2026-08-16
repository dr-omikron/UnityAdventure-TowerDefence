using UnityEngine;

namespace _Project.Develop.Runtime.Configs.Gameplay.Entities
{
    [CreateAssetMenu(menuName = "Configs/Gameplay/Entities/RadioactiveCloudConfig", fileName = "RadioactiveCloudConfig")]
    public class RadioactiveCloudConfig : EntityConfig
    {
        [field: SerializeField] public string PrefabPath { get; private set; } = "Entities/RadioactiveCloud";

        [field: SerializeField, Min(0)] public float Radius { get; private set; } = 20;
        [field: SerializeField, Min(0)] public float Damage { get; private set; } = 10;
        [field: SerializeField, Min(0)] public float DamageInterval { get; private set; } = 1;
    }
}
