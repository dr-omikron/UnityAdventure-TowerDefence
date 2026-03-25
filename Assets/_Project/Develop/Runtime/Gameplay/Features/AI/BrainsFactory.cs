using _Project.Develop.Runtime.Infrastructure.DI;

namespace _Project.Develop.Runtime.Gameplay.Features.AI
{
    public class BrainsFactory
    {
        private readonly DIContainer _container;

        public BrainsFactory(DIContainer container)
        {
            _container = container;
        }
    }
}
