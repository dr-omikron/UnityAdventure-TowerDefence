using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Develop.Runtime.Configs.Gameplay.Abilities;
using _Project.Develop.Runtime.Configs.Gameplay.Entities;
using _Project.Develop.Runtime.Configs.Gameplay.Levels;
using _Project.Develop.Runtime.Configs.Gameplay.Rewards;
using _Project.Develop.Runtime.Configs.Gameplay.Shop;
using _Project.Develop.Runtime.Configs.Meta.Wallet;
using _Project.Develop.Runtime.Utilities.AssetsManagement;
using UnityEngine;

namespace _Project.Develop.Runtime.Utilities.ConfigsManagement
{
    public class ResourcesConfigsLoader : IConfigsLoader
    {
        private readonly ResourcesAssetsLoader _resourcesAssetsLoader;

        private readonly Dictionary<Type, string> _configsResourcesPaths = new Dictionary<Type, string>
        {
            {typeof(StartWalletConfig), "Configs/Meta/Wallet/StartWalletConfig"},
            {typeof(CurrencyIconConfig), "Configs/Meta/Wallet/CurrencyIconConfig"},
            {typeof(WalletReplenishConfig), "Configs/Meta/Wallet/WalletReplenishConfig"},
            {typeof(LevelsListConfig), "Configs/Gameplay/Levels/LevelsListConfig"},
            {typeof(StationConfig), "Configs/Gameplay/Entities/StationConfig"},
            {typeof(SimpleEnemyConfig), "Configs/Gameplay/Entities/SimpleEnemyConfig"},
            {typeof(ShootingEnemyConfig), "Configs/Gameplay/Entities/ShootingEnemyConfig"},
            {typeof(TurretConfig), "Configs/Gameplay/Entities/TurretConfig"},
            {typeof(BombConfig), "Configs/Gameplay/Entities/BombConfig"},
            {typeof(RadioactiveCloudConfig), "Configs/Gameplay/Entities/RadioactiveCloudConfig"},
            {typeof(ShopConfig), "Configs/Gameplay/Shop/ShopConfig"},
            {typeof(ExplosionAbilityConfig), "Configs/Gameplay/Abilities/ExplosionAbilityConfig"},
            {typeof(WinRewardConfig), "Configs/Gameplay/Rewards/WinRewardConfig"}
        };

        public ResourcesConfigsLoader(ResourcesAssetsLoader resourcesAssetsLoader)
        {
            _resourcesAssetsLoader = resourcesAssetsLoader;
        }

        public IEnumerator LoadAsync(Action<Dictionary<Type, object>> onConfigsLoaded)
        {
            Dictionary<Type, object> loadedConfigs = new Dictionary<Type, object>();

            foreach (KeyValuePair<Type, string> resourcesPath in _configsResourcesPaths)
            {
                ScriptableObject config = _resourcesAssetsLoader.Load<ScriptableObject>(resourcesPath.Value);
                loadedConfigs.Add(resourcesPath.Key, config);
                yield return null;
            }

            onConfigsLoaded?.Invoke(loadedConfigs);
        }
    }
}
