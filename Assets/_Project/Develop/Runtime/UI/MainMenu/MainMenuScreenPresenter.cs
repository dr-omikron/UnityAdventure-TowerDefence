using System.Collections.Generic;
using _Project.Develop.Runtime.Gameplay.Infrastructure;
using _Project.Develop.Runtime.UI.Core;
using _Project.Develop.Runtime.UI.Wallet;
using _Project.Develop.Runtime.Utilities.CoroutinesManagement;
using _Project.Develop.Runtime.Utilities.SceneManagement;
using UnityEngine;

namespace _Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuScreenPresenter : IPresenter
    {
        private readonly MainMenuScreenView _mainMenuScreenView;
        private readonly ProjectPresenterFactory _projectPresenterFactory;
        private readonly List<IPresenter> _childPresenters = new List<IPresenter>();
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly SceneSwitcherService _sceneSwitcherService;
        private readonly int _levelsCount;

        public MainMenuScreenPresenter(
            MainMenuScreenView mainMenuScreenView, 
            ProjectPresenterFactory projectPresenterFactory, 
            ICoroutinesPerformer coroutinesPerformer, 
            SceneSwitcherService sceneSwitcherService, 
            int levelsCount)
        {
            _mainMenuScreenView = mainMenuScreenView;
            _projectPresenterFactory = projectPresenterFactory;
            _coroutinesPerformer = coroutinesPerformer;
            _sceneSwitcherService = sceneSwitcherService;
            _levelsCount = levelsCount;
        }

        public void Initialize()
        {
            _mainMenuScreenView.PlayGameButtonClicked += OnPlayGameButtonClicked;

            CreateWallet();

            foreach (IPresenter childPresenter in _childPresenters)
                childPresenter.Initialize();
        }

        public void Dispose()
        {
            _mainMenuScreenView.PlayGameButtonClicked -= OnPlayGameButtonClicked;

            foreach (IPresenter childPresenter in _childPresenters)
                childPresenter.Dispose();

            _childPresenters.Clear();
        }

        private void CreateWallet()
        {
            WalletPresenter walletPresenter = _projectPresenterFactory.CreateWalletPresenter(_mainMenuScreenView.WalletView);
            _childPresenters.Add(walletPresenter);
        }

        private void OnPlayGameButtonClicked()
        {
            int levelNumber = Random.Range(1, _levelsCount);
            _coroutinesPerformer.StartPerform(
                _sceneSwitcherService.ProcessSwitchTo(Scenes.Gameplay, new GameplayInputArgs(levelNumber)));
        }
    }
}
