using _Project.Develop.Runtime.Configs.Gameplay.Rewards;
using _Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using _Project.Develop.Runtime.Meta.Features.Wallet;

namespace _Project.Develop.Runtime.Gameplay.Features.Rewards
{
    public class WinRewardService
    {
        private readonly WinRewardConfig _config;
        private readonly StageProviderService _stageProviderService;
        private readonly WalletService _walletService;

        public WinRewardService(
            WinRewardConfig config,
            StageProviderService stageProviderService,
            WalletService walletService)
        {
            _config = config;
            _stageProviderService = stageProviderService;
            _walletService = walletService;
        }

        public void GrantReward()
        {
            int wavesCount = _stageProviderService.StageCount;

            foreach (CurrencyType currencyType in _config.RewardedCurrencies)
                _walletService.Add(currencyType, _config.GetRewardFor(currencyType, wavesCount));
        }
    }
}
