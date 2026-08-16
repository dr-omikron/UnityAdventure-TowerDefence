using System.Collections;
using _Project.Develop.Runtime.Infrastructure;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Meta.Features.Wallet;
using _Project.Develop.Runtime.Utilities.SceneManagement;

namespace _Project.Develop.Runtime.Meta.Infrastructure
{
    public class MainMenuBootstrap : SceneBootstrap
    {
        private DIContainer _container;
        private WalletReplenishService _walletReplenishService;

        public override void ProcessRegistration(DIContainer container, IInputSceneArgs sceneArgs = null)
        {
            _container = container;
            MainMenuContextRegistrations.Process(container);
        }

        public override IEnumerator Initialize()
        {
            _walletReplenishService = _container.Resolve<WalletReplenishService>();
            _walletReplenishService.ReplenishToMinimum();

            yield break;
        }

        public override void Run()
        {
        }

        private void Update()
        {
        }
    }
}
