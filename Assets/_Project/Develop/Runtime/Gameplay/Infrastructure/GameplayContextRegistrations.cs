using _Project.Develop.Runtime.Configs.Gameplay.Levels;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using _Project.Develop.Runtime.Gameplay.Features.AI;
using _Project.Develop.Runtime.Gameplay.Features.InputFeature;
using _Project.Develop.Runtime.Gameplay.Features.Sensors;
using _Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using _Project.Develop.Runtime.Gameplay.Features.Station;
using _Project.Develop.Runtime.Gameplay.States;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Utilities.AssetsManagement;
using _Project.Develop.Runtime.Utilities.ConfigsManagement;
using _Project.Develop.Runtime.Utilities.Physic;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameplayContextRegistrations
    {
        private static GameplayInputArgs _inputArgs;

        public static void Process(DIContainer container, GameplayInputArgs args)
        {
            _inputArgs = args;

            container.RegisterAsSingle<IInputService>(CreateDesktopInput);
            container.RegisterAsSingle(CreateStagesFactory);
            container.RegisterAsSingle(CreateStageProviderService);
            container.RegisterAsSingle(CreateBrainsFactory);
            container.RegisterAsSingle(CreateAIBrainContext);
            container.RegisterAsSingle(CreateGameplayStatesFactory);
            container.RegisterAsSingle(CreateGameplayStatesContext);
            container.RegisterAsSingle(CreateEntitiesFactory);
            container.RegisterAsSingle(CreateEntitiesLifeContext);
            container.RegisterAsSingle(CreateMonoEntityFactory).NonLazy();
            container.RegisterAsSingle(CreateColliderRegistryService);
            container.RegisterAsSingle(CreateScreenToWorldPointRaycastService);
            container.RegisterAsSingle(CreateContactsEntitiesFilter);
            container.RegisterAsSingle(CreateStationHolderService).NonLazy();
        }

        private static DesktopInput CreateDesktopInput(DIContainer c)
            => new DesktopInput();

        private static StagesFactory CreateStagesFactory(DIContainer c)
            => new StagesFactory(c);

        private static StageProviderService CreateStageProviderService(DIContainer c)
        {
            return new StageProviderService(c.Resolve<ConfigsProviderService>().GetConfig<LevelsListConfig>().GetBy(_inputArgs.LevelNumber),
                c.Resolve<StagesFactory>());
        }

        private static BrainsFactory CreateBrainsFactory(DIContainer c)
            => new BrainsFactory(c);

        private static AIBrainContext CreateAIBrainContext(DIContainer c)
            => new AIBrainContext();

        private static GameplayStatesFactory CreateGameplayStatesFactory(DIContainer c)
            => new GameplayStatesFactory(c);

        private static GameplayStatesContext CreateGameplayStatesContext(DIContainer c)
            => new GameplayStatesContext(c.Resolve<GameplayStatesFactory>().CreateGameplayStateMachine());

        private static EntitiesFactory CreateEntitiesFactory(DIContainer c)
            => new EntitiesFactory(c);

        private static EntitiesLifeContext CreateEntitiesLifeContext(DIContainer c)
            => new EntitiesLifeContext();
        
        private static MonoEntityFactory CreateMonoEntityFactory(DIContainer c)
        {
            return new MonoEntityFactory(
                c.Resolve<ResourcesAssetsLoader>(),
                c.Resolve<EntitiesLifeContext>(),
                c.Resolve<ColliderRegistryService>());
        }

        private static ColliderRegistryService CreateColliderRegistryService(DIContainer c)
            => new ColliderRegistryService();

        private static ScreenToWorldPointRaycastService CreateScreenToWorldPointRaycastService(DIContainer c)
            => new ScreenToWorldPointRaycastService(c.Resolve<IInputService>());

        private static ContactsEntitiesFilterService CreateContactsEntitiesFilter(DIContainer c)
            => new ContactsEntitiesFilterService(c.Resolve<ColliderRegistryService>());
        
        private static StationHolderService CreateStationHolderService(DIContainer c)
            => new StationHolderService(c.Resolve<EntitiesLifeContext>());
    }
}
