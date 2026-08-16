using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Develop.Runtime.Meta.Features.Wallet;
using UnityEngine;

namespace _Project.Develop.Runtime.Configs.Meta.Wallet
{
    [CreateAssetMenu(menuName = "Configs/Meta/Wallet/NewWalletReplenishConfig", fileName = "WalletReplenishConfig")]
    public class WalletReplenishConfig : ScriptableObject
    {
        [SerializeField] private List<CurrencyConfig> _minimalValues;

        public List<CurrencyType> ReplenishableCurrencies
            => _minimalValues.Select(config => config.Type).ToList();

        public int GetMinimalValueFor(CurrencyType currencyType)
            => _minimalValues.First(config => config.Type == currencyType).MinimalValue;

        [Serializable]
        private class CurrencyConfig
        {
            [field: SerializeField] public CurrencyType Type { get; private set; }
            [field: SerializeField, Min(0)] public int MinimalValue { get; private set; }
        }
    }
}
