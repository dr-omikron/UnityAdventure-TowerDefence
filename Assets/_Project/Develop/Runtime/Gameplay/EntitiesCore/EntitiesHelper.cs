using _Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using _Project.Develop.Runtime.Utilities.Reactive;

namespace _Project.Develop.Runtime.Gameplay.EntitiesCore
{
    public class EntitiesHelper
    {
        public static bool TryTakeDamageFor(Entity source, Entity target, float damage)
        {
            if(target.TryGetTakeDamageRequest(out ReactiveEvent<float> takeDamageRequest) == false)
                return false;

            if(source.TryGetTeam(out ReactiveVariable<Teams> sourceTeam) 
               && target.TryGetTeam(out ReactiveVariable<Teams> targetTeam))
            {
                if (sourceTeam.Value == targetTeam.Value)
                    return false;
            }

            takeDamageRequest.Invoke(damage);
            return true;
        }
    }
}
