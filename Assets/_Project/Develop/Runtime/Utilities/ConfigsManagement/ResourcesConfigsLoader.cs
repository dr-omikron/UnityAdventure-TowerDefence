using System;
using System.Collections;
using System.Collections.Generic;
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
            {typeof(StartWalletConfig), "Configs/Meta/Wallet/StartWalletConfig"}
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
