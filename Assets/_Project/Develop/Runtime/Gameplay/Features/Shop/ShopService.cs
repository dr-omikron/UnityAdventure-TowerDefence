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

        public ShopService(
            ConfigsProviderService configsProviderService,
            WalletService walletService,
            TurretsFactory turretsFactory,
            EntitiesFactory entitiesFactory,
            PurchasedEntitiesHolderService purchasedEntitiesHolderService)
        {
            _shopConfig = configsProviderService.GetConfig<ShopConfig>();
            _walletService = walletService;
            _turretsFactory = turretsFactory;
            _entitiesFactory = entitiesFactory;
            _purchasedEntitiesHolderService = purchasedEntitiesHolderService;
        }

        public IReadOnlyList<ShopItemConfig> Items => _shopConfig.Items;

        public bool CanBuy(ShopItemConfig item) => _walletService.Enough(item.CurrencyType, item.Cost);

        public bool NeedsFieldPlacement(ShopItemConfig item) => item.EntityConfig is BombConfig;

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
            if (item.EntityConfig is not BombConfig bombConfig)
                throw new ArgumentException($"Item { item.Title } does not support field placement");

            if (CanBuy(item) == false)
                return false;

            Entity bomb = _entitiesFactory.CreateBomb(position, bombConfig);

            _purchasedEntitiesHolderService.Add(bomb);
            _walletService.Spend(item.CurrencyType, item.Cost);

            return true;
        }
    }
}
