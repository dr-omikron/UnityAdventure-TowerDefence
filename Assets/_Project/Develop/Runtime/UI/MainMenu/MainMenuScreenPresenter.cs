using System.Collections.Generic;
using _Project.Develop.Runtime.UI.Core;
using _Project.Develop.Runtime.UI.Wallet;

namespace _Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuScreenPresenter : IPresenter
    {
        private readonly MainMenuScreenView _mainMenuScreenView;
        private readonly ProjectPresenterFactory _projectPresenterFactory;
        private readonly MainMenuPopupService _mainMenuPopupService;
        
        private readonly List<IPresenter> _childPresenters = new List<IPresenter>();

        public MainMenuScreenPresenter(
            MainMenuScreenView mainMenuScreenView, 
            ProjectPresenterFactory projectPresenterFactory, 
            MainMenuPopupService mainMenuPopupService)
        {
            _mainMenuScreenView = mainMenuScreenView;
            _projectPresenterFactory = projectPresenterFactory;
            _mainMenuPopupService = mainMenuPopupService;
        }

        public void Initialize()
        {
            _mainMenuScreenView.OpenLevelsMenuButtonClicked += OnOpenLevelsMenuButtonClicked;
            
            CreateWallet();

            foreach (IPresenter childPresenter in _childPresenters)
                childPresenter.Initialize();
        }

        public void Dispose()
        {
            _mainMenuScreenView.OpenLevelsMenuButtonClicked -= OnOpenLevelsMenuButtonClicked;

            foreach (IPresenter childPresenter in _childPresenters)
                childPresenter.Dispose();

            _childPresenters.Clear();
        }

        private void CreateWallet()
        {
            WalletPresenter walletPresenter = _projectPresenterFactory.CreateWalletPresenter(_mainMenuScreenView.WalletView);
            _childPresenters.Add(walletPresenter);
        }

        private void OnOpenLevelsMenuButtonClicked()
        {
            _mainMenuPopupService.OpenLevelsMenuPopup();
        }
    }
}
