using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.Features.ExplosionAbility;
using _Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using _Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.States
{
    public class StageProcessState : State, IUpdatableState
    {
        private readonly StageProviderService _stageProviderService;
        private readonly ExplosionAbilityService _explosionAbilityService;

        public StageProcessState(
            StageProviderService stageProviderService,
            ExplosionAbilityService explosionAbilityService)
        {
            _stageProviderService = stageProviderService;
            _explosionAbilityService = explosionAbilityService;
        }

        public override void Enter()
        {
            base.Enter();

            _stageProviderService.SwitchToNext();
            _stageProviderService.StartCurrent();

            Debug.Log("Entering To Stage Process State");
        }

        public override void Exit()
        {
            _stageProviderService.CleanupCurrent();
        }

        public void Update(float deltaTime)
        {
            _stageProviderService.UpdateCurrent(deltaTime);
            _explosionAbilityService.Update(deltaTime);
        }
    }
}
