using System;
using System.Collections;
using _Project.Develop.Runtime.Utilities.DataManagement.DataRepository;
using _Project.Develop.Runtime.Utilities.DataManagement.KeyStorage;
using _Project.Develop.Runtime.Utilities.DataManagement.Serializers;

namespace _Project.Develop.Runtime.Utilities.DataManagement
{
    public class SaveLoadService : ISaveLoadService
    {
        private readonly IDataSerializer _serializer;
        private readonly IDataKeyStorage _keysStorage;
        private readonly IDataRepository _dataRepository;

        public SaveLoadService(IDataSerializer serializer, IDataKeyStorage keysStorage, IDataRepository dataRepository)
        {
            _serializer = serializer;
            _keysStorage = keysStorage;
            _dataRepository = dataRepository;
        }

        public IEnumerator Load<TData>(Action<TData> onLoad) where TData : ISaveData
        {
            string key = _keysStorage.GetKeyFor<TData>();
            string serializedData = "";

            yield return _dataRepository.Read(key, result => serializedData = result);

            TData data = _serializer.Deserialize<TData>(serializedData);
            onLoad?.Invoke(data);
        }

        public IEnumerator Save<TData>(TData data) where TData : ISaveData
        {
            string serializedData = _serializer.Serialize(data);
            string key = _keysStorage.GetKeyFor<TData>();

            yield return _dataRepository.Write(key, serializedData);
        }

        public IEnumerator Remove<TData>(TData data) where TData : ISaveData
        {
            string key = _keysStorage.GetKeyFor<TData>();
            yield return _dataRepository.Remove(key);
        }

        public IEnumerator Exists<TData>(Action<bool> onExistsResult) where TData : ISaveData
        {
            string key = _keysStorage.GetKeyFor<TData>();
            yield return _dataRepository.Exists(key, result => onExistsResult?.Invoke(result));
        }
    }
}
