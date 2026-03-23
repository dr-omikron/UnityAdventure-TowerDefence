using System;
using System.Collections;
using System.Collections.Generic;

namespace _Project.Develop.Runtime.Utilities.DataManagement.DataProviders
{
    public abstract class DataProvider<TData> where TData : ISaveData
    {
        private readonly ISaveLoadService _saveLoadService;

        private readonly List<IDataWriter<TData>> _dataWriters = new List<IDataWriter<TData>>();
        private readonly List<IDataReader<TData>> _dataReaders = new List<IDataReader<TData>>();

        private TData _data;

        protected DataProvider(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        public void RegisterWriter(IDataWriter<TData> writer)
        {
            if(_dataWriters.Contains(writer))
                throw new ArgumentException(nameof(writer) + ". Writer already registered");

            _dataWriters.Add(writer);
        }

        public void RegisterReader(IDataReader<TData> reader)
        {
            if(_dataReaders.Contains(reader))
                throw new ArgumentException(nameof(reader) + ". Reader already registered");

            _dataReaders.Add(reader);
        }

        public IEnumerator LoadAsync()
        {
            yield return _saveLoadService.Load<TData>(loadedData => _data = loadedData);
            SendDataToReaders();
        }

        public IEnumerator SaveAsync()
        {
            UpdateDataFromWriters();
            yield return _saveLoadService.Save(_data);
        }

        public IEnumerator ExistsAsync(Action<bool> onExistsResult)
        {
            yield return _saveLoadService.Exists<TData>(result => onExistsResult?.Invoke(result));
        }

        public void Reset()
        {
            _data = GetOriginData();
            SendDataToReaders();
        }

        protected abstract TData GetOriginData();

        private void SendDataToReaders()
        {
            foreach (IDataReader<TData> reader in _dataReaders)
                reader.ReadFrom(_data);
        }

        private void UpdateDataFromWriters()
        {
            foreach (IDataWriter<TData> writer in _dataWriters)
                writer.WriteTo(_data);
        }
    }

}
