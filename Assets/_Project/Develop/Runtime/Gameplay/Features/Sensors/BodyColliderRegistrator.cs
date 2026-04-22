using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.Sensors
{
    public class BodyColliderRegistrator : MonoEntityRegistrator
    {
        [SerializeField] private CapsuleCollider _body;

        public override void Register(Entity entity)
        {
            entity.AddBodyCollider(_body);
        }
    }
}
