using System;
using System.Collections.Generic;
using _Project.Develop.Runtime.Configs.Gameplay.Entities;
using _Project.Develop.Runtime.Configs.Gameplay.Shop;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.Features.Turrets;
using _Project.Develop.Runtime.Meta.Features.Wallet;
using _Project.Develop.Runtime.Utilities.ConfigsManagement;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.Shop
{
    public class ShopService
    {
        private readonly ShopConfig _shopConfig;
        private readonly WalletService _walletService;
        private readonly TurretsFactory _turretsFactory;
        private readonly EntitiesFactory _entitiesFactory;
        private readonly PurchasedEntitiesHolderService _purchasedEntitiesHolderService;
        private readonly SingleWaveEntitiesHolderService _singleWaveEntitiesHolderService;

        public ShopService(
            ConfigsProviderService configsProviderService,
            WalletService walletService,
            TurretsFactory turretsFactory,
            EntitiesFactory entitiesFactory,
            PurchasedEntitiesHolderService purchasedEntitiesHolderService,
            SingleWaveEntitiesHolderService singleWaveEntitiesHolderService)
        {
            _shopConfig = configsProviderService.GetConfig<ShopConfig>();
            _walletService = walletService;
            _turretsFactory = turretsFactory;
            _entitiesFactory = entitiesFactory;
            _purchasedEntitiesHolderService = purchasedEntitiesHolderService;
            _singleWaveEntitiesHolderService = singleWaveEntitiesHolderService;
        }

        public IReadOnlyList<ShopItemConfig> Items => _shopConfig.Items;

        public bool CanBuy(ShopItemConfig item) => _walletService.Enough(item.CurrencyType, item.Cost);

        public bool NeedsFieldPlacement(ShopItemConfig item)
            => item.EntityConfig is BombConfig or RadioactiveCloudConfig;

        public bool TryBuy(ShopItemConfig item)
        {
            if (item.EntityConfig is not TurretConfig turretConfig)
                throw new ArgumentException($"Item { item.Title } can not be bought without field position");

            if (CanBuy(item) == false)
                return false;

            Entity turret = _turretsFactory.Create(turretConfig);

            _purchasedEntitiesHolderService.Add(turret);
            _walletService.Spend(item.CurrencyType, item.Cost);

            return true;
        }

        public bool TryBuyAt(ShopItemConfig item, Vector3 position)
        {
            if (NeedsFieldPlacement(item) == false)
                throw new ArgumentException($"Item { item.Title } does not support field placement");

            if (CanBuy(item) == false)
                return false;

            switch (item.EntityConfig)
            {
                case BombConfig bombConfig:
                    _purchasedEntitiesHolderService.Add(_entitiesFactory.CreateBomb(position, bombConfig));
                    break;

                case RadioactiveCloudConfig radioactiveCloudConfig:
                    _singleWaveEntitiesHolderService.Add(
                        _entitiesFactory.CreateRadioactiveCloud(position, radioactiveCloudConfig));
                    break;

                default:
                    throw new ArgumentException($"Not supported placeable config { item.EntityConfig.GetType() }");
            }

            _walletService.Spend(item.CurrencyType, item.Cost);

            return true;
        }
    }
}
