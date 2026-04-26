using System.Collections.Generic;
using System.Linq;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.Features.ApplyDamage;
using _Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using _Project.Develop.Runtime.Utilities.Conditions;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class NearestDamageableTargetSelector : ITargetSelector
    {
        private readonly Entity _source;
        private readonly Transform _sourceTransform;

        public NearestDamageableTargetSelector(Entity entity)
        {
            _source = entity;
            _sourceTransform = entity.Transform;
        }

        public Entity SelectTarget(IEnumerable<Entity> targets)
        {
            IEnumerable<Entity> selected = targets.Where(target =>
            {
                bool result = target.HasComponent<TakeDamageRequest>();

                if (target.TryGetCanApplyDamage(out ICompositeCondition canApplyDamage))
                {
                    result = result && canApplyDamage.Evaluate();
                }

                if (_source.TryGetTeam(out ReactiveVariable<Teams> sourceTeam)
                    && target.TryGetTeam(out ReactiveVariable<Teams> targetTeam))
                {
                    result = result && sourceTeam.Value != targetTeam.Value;
                }

                result = result && target != _source;

                return result;
            });

            if (selected.Any() == false)
                return null;

            Entity closestTarget = selected.First();
            float minDistance = GetDistanceTo(closestTarget);

            foreach (Entity target in selected)
            {
                float distance = GetDistanceTo(target);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestTarget = target;
                }
            }

            return closestTarget;
        }

        private float GetDistanceTo(Entity target) => (target.Transform.position - _sourceTransform.position).magnitude;
    }
}
