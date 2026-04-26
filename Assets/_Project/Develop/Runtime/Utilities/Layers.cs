using UnityEngine;

namespace _Project.Develop.Runtime.Utilities
{
    public class Layers
    {
        public static readonly int Entity = LayerMask.NameToLayer("Entity");
        public static readonly LayerMask EntityMask = 1 << Entity;

        public static readonly int Enemy = LayerMask.NameToLayer("Enemy");
        public static readonly LayerMask EnemyMask = 1 << Enemy;
    }
}
