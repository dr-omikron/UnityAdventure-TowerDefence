using _Project.Develop.Runtime.Configs.Gameplay.Shop;
using _Project.Develop.Runtime.Gameplay.Features.Shop;
using _Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using _Project.Develop.Runtime.Gameplay.Infrastructure;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Meta.Features.Wallet;
using _Project.Develop.Runtime.UI.CommonViews;
using _Project.Develop.Runtime.UI.Core;
using _Project.Develop.Runtime.UI.Gameplay.ResultPopups;
using _Project.Develop.Runtime.UI.Gameplay.Shop;
using _Project.Develop.Runtime.Utilities.CoroutinesManagement;
using _Project.Develop.Runtime.Utilities.SceneManagement;

namespace _Project.Develop.Runtime.UI.Gameplay
{
    public class GameplayPresentersFactory
    {
        private readonly DIContainer _container;
        private readonly GameplayInputArgs _gameplayInputArgs;

        public GameplayPresentersFactory(DIContainer container, GameplayInputArgs gameplayInputArgs)
        {
            _container = container;
            _gameplayInputArgs = gameplayInputArgs;
        }

        public WinPopupPresenter CreateWinPopupPresenter(WinPopupView view)
        {
            return new WinPopupPresenter(
                _container.Resolve<ICoroutinesPerformer>(),
                view,
                _container.Resolve<SceneSwitcherService>());
        }

        public DefeatPopupPresenter CreateDefeatPopupPresenter(DefeatPopupView view)
        {
            return new DefeatPopupPresenter(
                _container.Resolve<ICoroutinesPerformer>(),
                view,
                _container.Resolve<SceneSwitcherService>(),
                _gameplayInputArgs);
        }

        public ShopPopupPresenter CreateShopPopupPresenter(ShopPopupView view)
        {
            return new ShopPopupPresenter(
                _container.Resolve<ICoroutinesPerformer>(),
                _container.Resolve<ShopService>(),
                _container.Resolve<FieldPlacementService>(),
                this,
                _container.Resolve<ViewsFactory>(),
                view);
        }

        public ShopTilePresenter CreateShopTilePresenter(ShopTileView view, ShopItemConfig item)
        {
            return new ShopTilePresenter(
                view,
                item,
                _container.Resolve<ShopService>(),
                _container.Resolve<WalletService>());
        }

        public StagesPresenter CreateStagesPresenter(IconTextView view)
        {
            return new StagesPresenter(view, _container.Resolve<StageProviderService>());
        }
    }
}
