using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.OrbitMovement
{
    public class OrbitCenter : IEntityComponent
    {
        public ReactiveVariable<Vector3> Value;
    }

    public class OrbitRadius : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class OrbitAngle : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class OrbitAngularSpeed : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }
}
