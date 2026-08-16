using System.Collections.Generic;
using _Project.Develop.Runtime.UI.CommonViews;
using _Project.Develop.Runtime.UI.Core;
using _Project.Develop.Runtime.UI.Gameplay.HealthBars;
using _Project.Develop.Runtime.UI.Wallet;

namespace _Project.Develop.Runtime.UI.Gameplay
{
    public class GameplayScreenPresenter : IPresenter
    {
        private readonly GameplayScreenView _gameplayScreenView;
        private readonly ProjectPresenterFactory _projectPresenterFactory;
        private readonly GameplayPresentersFactory _gameplayPresentersFactory;
        private readonly ViewsFactory _viewsFactory;
        private readonly List<IPresenter> _childPresenters = new List<IPresenter>();

        public GameplayScreenPresenter(
            GameplayScreenView gameplayScreenView,
            ProjectPresenterFactory projectPresenterFactory,
            GameplayPresentersFactory gameplayPresentersFactory,
            ViewsFactory viewsFactory)
        {
            _gameplayScreenView = gameplayScreenView;
            _projectPresenterFactory = projectPresenterFactory;
            _gameplayPresentersFactory = gameplayPresentersFactory;
            _viewsFactory = viewsFactory;
        }

        public void Initialize()
        {
            CreateWallet();
            CreateStages();
            CreateEntitiesHealthBars();
            CreateReloadBar();

            foreach (IPresenter childPresenter in _childPresenters)
                childPresenter.Initialize();
        }

        public void Dispose()
        {
            foreach (IPresenter childPresenter in _childPresenters)
                childPresenter.Dispose();

            _childPresenters.Clear();
        }

        private void CreateWallet()
        {
            WalletPresenter walletPresenter =
                _projectPresenterFactory.CreateWalletPresenter(_gameplayScreenView.WalletView);

            _childPresenters.Add(walletPresenter);
        }

        private void CreateStages()
        {
            StagesPresenter stagesPresenter =
                _gameplayPresentersFactory.CreateStagesPresenter(_gameplayScreenView.StagesView);

            _childPresenters.Add(stagesPresenter);
        }

        private void CreateEntitiesHealthBars()
        {
            EntitiesHealthBarsPresenter healthBarsPresenter = _gameplayPresentersFactory
                .CreateEntitiesHealthBarsPresenter(_gameplayScreenView.EntitiesHealthDisplay);

            _childPresenters.Add(healthBarsPresenter);
        }

        private void CreateReloadBar()
        {
            BarWithText reloadBarView = _viewsFactory
                .Create<BarWithText>(ViewIDs.ReloadBar, _gameplayScreenView.ReloadBarContainer);

            _childPresenters.Add(_gameplayPresentersFactory.CreateReloadBarPresenter(reloadBarView));
        }
    }
}
