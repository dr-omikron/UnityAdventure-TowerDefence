using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Common
{
    public class RigidbodyComponent : IEntityComponent
    {
        public Rigidbody Value;
    }

    public class TransformComponent : IEntityComponent
    {
        public Transform Value;
    }
}
