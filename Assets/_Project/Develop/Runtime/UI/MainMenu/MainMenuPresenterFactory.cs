using _Project.Develop.Runtime.Configs.Gameplay.Levels;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Utilities.ConfigsManagement;
using _Project.Develop.Runtime.Utilities.CoroutinesManagement;
using _Project.Develop.Runtime.Utilities.SceneManagement;

namespace _Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuPresenterFactory
    {
        private readonly DIContainer _container;

        public MainMenuPresenterFactory(DIContainer container)
        {
            _container = container;
        }

        public MainMenuScreenPresenter CreateMainMenuScreen(MainMenuScreenView view)
        {
            return new MainMenuScreenPresenter(
                view,
                _container.Resolve<ProjectPresenterFactory>(),
                _container.Resolve<ICoroutinesPerformer>(),
                _container.Resolve<SceneSwitcherService>(),
                _container.Resolve<ConfigsProviderService>().GetConfig<LevelsListConfig>().Levels.Count);
        }

    }
}
