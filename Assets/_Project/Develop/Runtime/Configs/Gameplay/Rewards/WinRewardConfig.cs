using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Develop.Runtime.Meta.Features.Wallet;
using UnityEngine;

namespace _Project.Develop.Runtime.Configs.Gameplay.Rewards
{
    [CreateAssetMenu(menuName = "Configs/Gameplay/Rewards/NewWinRewardConfig", fileName = "WinRewardConfig")]
    public class WinRewardConfig : ScriptableObject
    {
        [SerializeField] private List<CurrencyRewardConfig> _rewards;

        public List<CurrencyType> RewardedCurrencies => _rewards.Select(reward => reward.Type).ToList();

        public int GetRewardFor(CurrencyType currencyType, int wavesCount)
        {
            if (wavesCount < 0)
                throw new ArgumentOutOfRangeException(nameof(wavesCount));

            return _rewards.First(reward => reward.Type == currencyType).AmountPerWave * wavesCount;
        }

        [Serializable]
        private class CurrencyRewardConfig
        {
            [field: SerializeField] public CurrencyType Type { get; private set; }
            [field: SerializeField, Min(0)] public int AmountPerWave { get; private set; }
        }
    }
}
