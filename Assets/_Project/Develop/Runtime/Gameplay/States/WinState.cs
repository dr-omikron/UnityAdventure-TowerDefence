using _Project.Develop.Runtime.Gameplay.Features.InputFeature;
using _Project.Develop.Runtime.Gameplay.Features.Rewards;
using _Project.Develop.Runtime.Gameplay.Features.Shop;
using _Project.Develop.Runtime.UI.Gameplay;
using _Project.Develop.Runtime.Utilities.CoroutinesManagement;
using _Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using _Project.Develop.Runtime.Utilities.StateMachineCore;

namespace _Project.Develop.Runtime.Gameplay.States
{
    public class WinState : EndGameState, IUpdatableState
    {
        private readonly GameplayPopupService _popupService;
        private readonly WinRewardService _winRewardService;
        private readonly PlayerDataProvider _playerDataProvider;
        private readonly ICoroutinesPerformer _coroutinesPerformer;

        public WinState(
            IInputService inputService,
            PurchasedEntitiesHolderService purchasedEntitiesHolderService,
            SingleWaveEntitiesHolderService singleWaveEntitiesHolderService,
            GameplayPopupService popupService,
            WinRewardService winRewardService,
            PlayerDataProvider playerDataProvider,
            ICoroutinesPerformer coroutinesPerformer)
            : base(inputService, purchasedEntitiesHolderService, singleWaveEntitiesHolderService)
        {
            _popupService = popupService;
            _winRewardService = winRewardService;
            _playerDataProvider = playerDataProvider;
            _coroutinesPerformer = coroutinesPerformer;
        }

        public override void Enter()
        {
            base.Enter();

            _winRewardService.GrantReward();
            _coroutinesPerformer.StartPerform(_playerDataProvider.SaveAsync());

            _popupService.OpenWinPopup();

            //LevelProgressionService.AddToLevelCompleted(GameplayInputArgs.LevelNumber)
        }

        public void Update(float deltaTime) { }
    }
}
