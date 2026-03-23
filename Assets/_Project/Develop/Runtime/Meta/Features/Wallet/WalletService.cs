using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Develop.Runtime.Utilities.DataManagement;
using _Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using _Project.Develop.Runtime.Utilities.Reactive;

namespace _Project.Develop.Runtime.Meta.Features.Wallet
{
    public class WalletService : IDataReader<PlayerData>, IDataWriter<PlayerData>
    {
        private readonly Dictionary<CurrencyType, ReactiveVariable<int>> _currencies;

        public WalletService(
            Dictionary<CurrencyType, ReactiveVariable<int>> currencies,
            PlayerDataProvider playerDataProvider)
        {
            _currencies = new Dictionary<CurrencyType, ReactiveVariable<int>>(currencies);
            playerDataProvider.RegisterReader(this);
            playerDataProvider.RegisterWriter(this);
        }

        public List<CurrencyType> AvailableCurrencies => _currencies.Keys.ToList();
        public IReadOnlyVariable<int> GetCurrency(CurrencyType type) => _currencies[type];

        public bool Enough(CurrencyType type, int amount)
        {
            if(amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            return _currencies[type].Value >= amount;
        }

        public void Add(CurrencyType type, int amount)
        {
            if(amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            _currencies[type].Value += amount;
        }

        public void Spend(CurrencyType type, int amount)
        {
            if(Enough(type, amount) == false)
                throw new InvalidOperationException("Not enough currency: " + type);

            if(amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            _currencies[type].Value -= amount;
        }

        public void ReadFrom(PlayerData data)
        {
            foreach (KeyValuePair<CurrencyType, int> currency in data.WalletData)
            {
                if(_currencies.ContainsKey(currency.Key))
                    _currencies[currency.Key].Value = currency.Value;
                else
                    _currencies.Add(currency.Key, new ReactiveVariable<int>(currency.Value));
            }
        }

        public void WriteTo(PlayerData data)
        {
            foreach (KeyValuePair<CurrencyType, ReactiveVariable<int>> currency in _currencies)
            {
                if(data.WalletData.ContainsKey(currency.Key))
                    data.WalletData[currency.Key] = currency.Value.Value;
                else
                    data.WalletData.Add(currency.Key, currency.Value.Value);
            }
        }
    }
}
