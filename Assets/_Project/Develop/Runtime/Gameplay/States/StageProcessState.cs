using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using _Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.States
{
    public class StageProcessState : State, IUpdatableState
    {
        private readonly StageProviderService _stageProviderService;

        public StageProcessState(StageProviderService stageProviderService)
        {
            _stageProviderService = stageProviderService;
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
        }
    }
}
