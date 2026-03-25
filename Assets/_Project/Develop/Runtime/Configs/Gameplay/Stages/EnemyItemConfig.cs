using System;
using UnityEngine;

namespace _Project.Develop.Runtime.Configs.Gameplay.Stages
{
    [Serializable]

    public class EnemyItemConfig
    {
        //[field: SerializeField] public EnemyConfig
        [field: SerializeField] public Vector3 SpawnPosition { get; private set; }
    }
}
