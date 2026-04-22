using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Utilities.Reactive;

namespace _Project.Develop.Runtime.Gameplay.Features.Station
{
    public class IsStation : IEntityComponent
    {
        public ReactiveVariable<bool> Value;
    }
}
