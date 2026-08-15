using _Project.Develop.Runtime.Configs.Gameplay.Abilities;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.Features.InputFeature;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Utilities.ConfigsManagement;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.ExplosionAbility
{
    public class ExplosionAbilityService : IInitializable
    {
        private readonly EntitiesFactory _entitiesFactory;
        private readonly FieldClickService _fieldClickService;
        private readonly ExplosionAbilityConfig _config;

        private Entity _caster;

        public ExplosionAbilityService(
            EntitiesFactory entitiesFactory,
            FieldClickService fieldClickService,
            ConfigsProviderService configsProviderService)
        {
            _entitiesFactory = entitiesFactory;
            _fieldClickService = fieldClickService;
            _config = configsProviderService.GetConfig<ExplosionAbilityConfig>();
        }

        public void Initialize()
        {
            _caster = _entitiesFactory.CreateExplosionCaster(_config);
        }

        public void Update(float deltaTime)
        {
            if (_fieldClickService.TryGetClickedPoint(out Vector3 point) == false)
                return;

            _caster.StartAreaAttackRequest.Invoke(point);
        }
    }
}
