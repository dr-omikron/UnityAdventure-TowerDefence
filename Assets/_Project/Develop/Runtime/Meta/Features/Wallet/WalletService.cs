using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Develop.Runtime.Utilities.Reactive;

namespace _Project.Develop.Runtime.Meta.Features.Wallet
{
    public class WalletService
    {
        private readonly Dictionary<CurrencyType, ReactiveVariable<int>> _currencies;

        public WalletService(Dictionary<CurrencyType, ReactiveVariable<int>> currencies)
        {
            _currencies = new Dictionary<CurrencyType, ReactiveVariable<int>>(currencies);
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
    }
}
