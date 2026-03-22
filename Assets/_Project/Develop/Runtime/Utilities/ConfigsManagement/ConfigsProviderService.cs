using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace _Project.Develop.Runtime.Utilities.ConfigsManagement
{
    public class ConfigsProviderService
    {
        private readonly Dictionary<Type, object> _configs = new Dictionary<Type, object>();
        private readonly IConfigsLoader[] _configsLoaders;

        public ConfigsProviderService(params IConfigsLoader[] configsLoaders)
        {
            _configsLoaders = configsLoaders;
        }

        public IEnumerator LoadAsync()
        {
            foreach (var loader in _configsLoaders)
                yield return loader.LoadAsync(loadedConfigs => _configs.AddRange(loadedConfigs));
        }

        public T GetConfig<T>() where T : class
        {
            if (_configs.ContainsKey(typeof(T)) == false)
                throw new InvalidOperationException($"Not found config by {typeof(T)}");

            return (T)_configs[typeof(T)];
        }
    }
}
