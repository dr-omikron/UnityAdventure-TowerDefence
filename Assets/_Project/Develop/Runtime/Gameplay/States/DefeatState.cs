using _Project.Develop.Runtime.Gameplay.Features.InputFeature;
using _Project.Develop.Runtime.Gameplay.Features.Shop;
using _Project.Develop.Runtime.UI.Gameplay;
using _Project.Develop.Runtime.Utilities.StateMachineCore;

namespace _Project.Develop.Runtime.Gameplay.States
{
    public class DefeatState : EndGameState, IUpdatableState
    {
        private readonly GameplayPopupService _popupService;

        public DefeatState(
            IInputService inputService,
            PurchasedEntitiesHolderService purchasedEntitiesHolderService,
            SingleWaveEntitiesHolderService singleWaveEntitiesHolderService,
            GameplayPopupService popupService)
            : base(inputService, purchasedEntitiesHolderService, singleWaveEntitiesHolderService)
        {
            _popupService = popupService;
        }

        public override void Enter()
        {
            base.Enter();

            _popupService.OpenDefeatPopup();
        }

        public void Update(float deltaTime) { }
    }
}
