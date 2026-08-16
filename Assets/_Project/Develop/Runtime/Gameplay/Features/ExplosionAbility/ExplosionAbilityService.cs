using _Project.Develop.Runtime.Configs.Gameplay.Abilities;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.Features.InputFeature;
using _Project.Develop.Runtime.Utilities.ConfigsManagement;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.ExplosionAbility
{
    public class ExplosionAbilityService
    {
        private readonly FieldClickService _fieldClickService;
        private readonly Entity _caster;

        public ExplosionAbilityService(
            EntitiesFactory entitiesFactory,
            FieldClickService fieldClickService,
            ConfigsProviderService configsProviderService)
        {
            _fieldClickService = fieldClickService;

            ExplosionAbilityConfig config = configsProviderService.GetConfig<ExplosionAbilityConfig>();
            _caster = entitiesFactory.CreateExplosionCaster(config);
        }

        public IReadOnlyVariable<float> CooldownRemainingTime => _caster.AttackCooldownCurrentTime;
        public float CooldownTotalTime => _caster.AttackCooldownInitialTime.Value;

        public void Update(float deltaTime)
        {
            if (_fieldClickService.TryGetClickedPoint(out Vector3 point) == false)
                return;

            _caster.StartAreaAttackRequest.Invoke(point);
        }
    }
}
