using _Project.Develop.Runtime.Configs.Gameplay.Levels;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using _Project.Develop.Runtime.Gameplay.Features.AI;
using _Project.Develop.Runtime.Gameplay.Features.Enemies;
using _Project.Develop.Runtime.Gameplay.Features.ExplosionAbility;
using _Project.Develop.Runtime.Gameplay.Features.InputFeature;
using _Project.Develop.Runtime.Gameplay.Features.Sensors;
using _Project.Develop.Runtime.Gameplay.Features.Shop;
using _Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using _Project.Develop.Runtime.Gameplay.Features.Station;
using _Project.Develop.Runtime.Gameplay.Features.Turrets;
using _Project.Develop.Runtime.Gameplay.States;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Meta.Features.Wallet;
using _Project.Develop.Runtime.UI;
using _Project.Develop.Runtime.UI.Core;
using _Project.Develop.Runtime.UI.Gameplay;
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
            container.RegisterAsSingle(CreateStationFactory);
            container.RegisterAsSingle(CreateEnemiesFactory);
            container.RegisterAsSingle(CreateStageFactory);
            container.RegisterAsSingle(CreateStageProvider);
            container.RegisterAsSingle(CreateTurretsHolderService).NonLazy();
            container.RegisterAsSingle(CreateTurretsFactory);
            container.RegisterAsSingle(CreatePurchasedEntitiesHolderService).NonLazy();
            container.RegisterAsSingle(CreateShopService);
            container.RegisterAsSingle(CreateFieldClickService);
            container.RegisterAsSingle(CreateFieldPlacementService);
            container.RegisterAsSingle(CreateExplosionAbilityService).NonLazy();
            container.RegisterAsSingle(CreateGameplayUIRoot).NonLazy();
            container.RegisterAsSingle(CreateGameplayPresentersFactory);
            container.RegisterAsSingle(CreateGameplayPopupService);
            container.RegisterAsSingle(CreateGameplayScreenPresenter).NonLazy();
        }

        private static DesktopInput CreateDesktopInput(DIContainer c)
            => new DesktopInput();

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

        private static StationFactory CreateStationFactory(DIContainer c)
            => new StationFactory(c);

        private static EnemiesFactory CreateEnemiesFactory(DIContainer c)
            => new EnemiesFactory(c);

        private static StagesFactory CreateStageFactory(DIContainer c)
            => new StagesFactory(c);

        private static StageProviderService CreateStageProvider(DIContainer c)
        {
            LevelsListConfig levelsListConfig = c.Resolve<ConfigsProviderService>().GetConfig<LevelsListConfig>();
            return new StageProviderService(levelsListConfig.GetBy(_inputArgs.LevelNumber),
                c.Resolve<StagesFactory>());
        }

        private static TurretsHolderService CreateTurretsHolderService(DIContainer c)
            => new TurretsHolderService(c.Resolve<EntitiesLifeContext>());

        private static TurretsFactory CreateTurretsFactory(DIContainer c)
            => new TurretsFactory(c);

        private static PurchasedEntitiesHolderService CreatePurchasedEntitiesHolderService(DIContainer c)
            => new PurchasedEntitiesHolderService(c.Resolve<EntitiesLifeContext>());

        private static ShopService CreateShopService(DIContainer c)
        {
            return new ShopService(
                c.Resolve<ConfigsProviderService>(),
                c.Resolve<WalletService>(),
                c.Resolve<TurretsFactory>(),
                c.Resolve<EntitiesFactory>(),
                c.Resolve<PurchasedEntitiesHolderService>());
        }

        private static FieldClickService CreateFieldClickService(DIContainer c)
        {
            return new FieldClickService(
                c.Resolve<IInputService>(),
                c.Resolve<ScreenToWorldPointRaycastService>());
        }

        private static FieldPlacementService CreateFieldPlacementService(DIContainer c)
            => new FieldPlacementService(c.Resolve<FieldClickService>());

        private static ExplosionAbilityService CreateExplosionAbilityService(DIContainer c)
        {
            return new ExplosionAbilityService(
                c.Resolve<EntitiesFactory>(),
                c.Resolve<FieldClickService>(),
                c.Resolve<ConfigsProviderService>());
        }

        private static GameplayUIRoot CreateGameplayUIRoot(DIContainer c)
        {
            ResourcesAssetsLoader resourcesAssetsLoader = c.Resolve<ResourcesAssetsLoader>();

            GameplayUIRoot gameplayUIRootPrefab = resourcesAssetsLoader
                .Load<GameplayUIRoot>("UI/Gameplay/GameplayUIRoot");

            return Object.Instantiate(gameplayUIRootPrefab);
        }

        private static GameplayPresentersFactory CreateGameplayPresentersFactory(DIContainer c)
            => new GameplayPresentersFactory(c, _inputArgs);

        private static GameplayPopupService CreateGameplayPopupService(DIContainer c)
        {
            return new GameplayPopupService(
                c.Resolve<ViewsFactory>(),
                c.Resolve<ProjectPresenterFactory>(),
                c.Resolve<GameplayUIRoot>(),
                c.Resolve<GameplayPresentersFactory>());
        }

        private static GameplayScreenPresenter CreateGameplayScreenPresenter(DIContainer c)
        {
            GameplayUIRoot uiRoot = c.Resolve<GameplayUIRoot>();

            GameplayScreenView view = c
                .Resolve<ViewsFactory>()
                .Create<GameplayScreenView>(ViewIDs.GameplayScreen, uiRoot.HUDLayer);

            return new GameplayScreenPresenter(
                view,
                c.Resolve<ProjectPresenterFactory>(),
                c.Resolve<GameplayPresentersFactory>());
        }
    }
}
