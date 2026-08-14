using _Project.Develop.Runtime.Gameplay.Features.InputFeature;
using _Project.Develop.Runtime.Gameplay.Features.Shop;
using _Project.Develop.Runtime.Utilities.StateMachineCore;

namespace _Project.Develop.Runtime.Gameplay.States
{
    public abstract class EndGameState : State
    {
        private readonly IInputService _inputService;
        private readonly PurchasedEntitiesHolderService _purchasedEntitiesHolderService;

        protected EndGameState(
            IInputService inputService,
            PurchasedEntitiesHolderService purchasedEntitiesHolderService)
        {
            _inputService = inputService;
            _purchasedEntitiesHolderService = purchasedEntitiesHolderService;
        }

        public override void Enter()
        {
            base.Enter();

            _inputService.IsEnabled = false;
            _purchasedEntitiesHolderService.ReleaseAll();
        }

        public override void Exit()
        {
            base.Exit();
            _inputService.IsEnabled = true;
        }
    }
}
