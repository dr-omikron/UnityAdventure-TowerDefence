using _Project.Develop.Runtime.Infrastructure.DI;

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
                _container.Resolve<MainMenuPopupService>());
        }

    }
}
