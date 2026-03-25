using _Project.Develop.Runtime.Gameplay.Features.InputFeature;
using _Project.Develop.Runtime.Infrastructure.DI;

namespace _Project.Develop.Runtime.Gameplay.States
{
    public class GameplayStatesFactory
    {
        private readonly DIContainer _container;

        public GameplayStatesFactory(DIContainer container)
        {
            _container = container;
        }

        public WinState CreateWinState()
        {
            return new WinState(_container.Resolve<IInputService>());
        }

        public DefeatState CreateDefeatState()
        {
            return new DefeatState(_container.Resolve<IInputService>());
        }

        public GameplayStateMachine CreateGameplayStateMachine()
        {
            return new GameplayStateMachine();
        }
    }
}
