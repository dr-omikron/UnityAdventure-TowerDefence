using _Project.Develop.Runtime.Gameplay.Features.Shop;
using _Project.Develop.Runtime.UI.Gameplay;
using _Project.Develop.Runtime.UI.Gameplay.Shop;
using _Project.Develop.Runtime.Utilities.StateMachineCore;

namespace _Project.Develop.Runtime.Gameplay.States
{
    public class PreparationState : State, IUpdatableState
    {
        private readonly GameplayPopupService _popupService;
        private readonly FieldPlacementService _fieldPlacementService;

        private ShopPopupPresenter _shopPopupPresenter;
        private bool _isReadyToNextStage;

        public PreparationState(GameplayPopupService popupService, FieldPlacementService fieldPlacementService)
        {
            _popupService = popupService;
            _fieldPlacementService = fieldPlacementService;
        }

        public bool IsReadyToNextStage => _isReadyToNextStage;

        public override void Enter()
        {
            base.Enter();

            _isReadyToNextStage = false;

            _shopPopupPresenter = _popupService.OpenShopPopup();
            _shopPopupPresenter.NextRequested += OnNextRequested;
        }

        public override void Exit()
        {
            base.Exit();

            _fieldPlacementService.Cancel();

            if (_shopPopupPresenter == null)
                return;

            ShopPopupPresenter closingPopup = _shopPopupPresenter;
            _shopPopupPresenter = null;

            closingPopup.NextRequested -= OnNextRequested;
            _popupService.ClosePopup(closingPopup);
        }

        public void Update(float deltaTime)
        {
            _fieldPlacementService.Update(deltaTime);
        }

        private void OnNextRequested()
        {
            _shopPopupPresenter.NextRequested -= OnNextRequested;
            _shopPopupPresenter = null;

            _isReadyToNextStage = true;
        }
    }
}
