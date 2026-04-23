using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Utilities;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.Sensors
{
    public class BodyCollider : IEntityComponent
    {
        public CapsuleCollider Value;
    }

    public class ContactDetectingMask : IEntityComponent
    {
        public LayerMask Value;
    }

    public class ContactCollidersBuffer : IEntityComponent
    {
        public Buffer<Collider> Value;
    }

    public class ContactEntitiesBuffer : IEntityComponent
    {
        public Buffer<Entity> Value;
    }

    public class AreaDetectingMask : IEntityComponent
    {
        public LayerMask Value;
    }

    public class AreaCollidersBuffer : IEntityComponent
    {
        public Buffer<Collider> Value;
    }

    public class AreaEntitiesBuffer : IEntityComponent
    {
        public Buffer<Entity> Value;
    }

    public class AreaDetectingRadius : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class EndCastAreaPositionEvent : IEntityComponent
    {
        public ReactiveEvent Value;
    }

    public class CastAreaPositionEvent : IEntityComponent
    {
        public ReactiveEvent<Vector3> Value;
    }

    public class DeathMask : IEntityComponent
    {
        public LayerMask Value;
    }

    public class IsTouchingDeathMask : IEntityComponent
    {
        public ReactiveVariable<bool> Value;
    }
}
