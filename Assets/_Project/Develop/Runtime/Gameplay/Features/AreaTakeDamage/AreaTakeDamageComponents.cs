using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Utilities.Reactive;

namespace _Project.Develop.Runtime.Gameplay.Features.AreaTakeDamage
{
    public class AreaDamage : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }
}
