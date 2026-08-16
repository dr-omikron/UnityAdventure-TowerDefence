using _Project.Develop.Runtime.Configs.Meta.Wallet;

namespace _Project.Develop.Runtime.Meta.Features.Wallet
{
    public class WalletReplenishService
    {
        private readonly WalletReplenishConfig _config;
        private readonly WalletService _walletService;

        public WalletReplenishService(WalletReplenishConfig config, WalletService walletService)
        {
            _config = config;
            _walletService = walletService;
        }

        public void ReplenishToMinimum()
        {
            foreach (CurrencyType currencyType in _config.ReplenishableCurrencies)
                ReplenishToMinimum(currencyType);
        }

        private void ReplenishToMinimum(CurrencyType currencyType)
        {
            int minimalValue = _config.GetMinimalValueFor(currencyType);
            int currentValue = _walletService.GetCurrency(currencyType).Value;

            if (currentValue >= minimalValue)
                return;

            _walletService.Add(currencyType, minimalValue - currentValue);
        }
    }
}
