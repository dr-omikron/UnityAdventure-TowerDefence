using System;
using _Project.Develop.Runtime.Configs.Gameplay.Shop;
using _Project.Develop.Runtime.Gameplay.Features.Shop;
using _Project.Develop.Runtime.Meta.Features.Wallet;
using _Project.Develop.Runtime.UI.Core;

namespace _Project.Develop.Runtime.UI.Gameplay.Shop
{
    public class ShopTilePresenter : ISubscribePresenter
    {
        public event Action<ShopTilePresenter> SelectRequested;

        private readonly ShopTileView _shopTileView;
        private readonly ShopItemConfig _item;
        private readonly ShopService _shopService;
        private readonly WalletService _walletService;

        private IDisposable _currencyChangedDisposable;

        public ShopTilePresenter(
            ShopTileView shopTileView,
            ShopItemConfig item,
            ShopService shopService,
            WalletService walletService)
        {
            _shopTileView = shopTileView;
            _item = item;
            _shopService = shopService;
            _walletService = walletService;
        }

        public ShopTileView ShopTileView => _shopTileView;
        public ShopItemConfig Item => _item;

        public void Initialize()
        {
            _shopTileView.SetTitle(_item.Title);
            _shopTileView.SetCost(_item.Cost.ToString());

            if (_item.Icon != null)
                _shopTileView.SetIcon(_item.Icon);

            RefreshAvailability();
        }

        public void Subscribe()
        {
            _shopTileView.Clicked += OnViewClicked;

            _currencyChangedDisposable = _walletService
                .GetCurrency(_item.CurrencyType)
                .Subscribe(OnCurrencyChanged);
        }

        public void UnSubscribe()
        {
            _shopTileView.Clicked -= OnViewClicked;

            _currencyChangedDisposable?.Dispose();
            _currencyChangedDisposable = null;
        }

        public void Select() => _shopTileView.SetSelected();

        public void Deselect() => _shopTileView.SetDeselected();

        public void Dispose()
        {
            UnSubscribe();
        }

        private void RefreshAvailability()
        {
            if (_shopService.CanBuy(_item))
                _shopTileView.SetActive();
            else
                _shopTileView.SetBlock();
        }

        private void OnCurrencyChanged(int oldValue, int newValue) => RefreshAvailability();

        private void OnViewClicked() => SelectRequested?.Invoke(this);
    }
}
