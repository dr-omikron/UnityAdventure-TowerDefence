using System;
using System.Collections.Generic;
using _Project.Develop.Runtime.Configs.Gameplay.Shop;
using _Project.Develop.Runtime.Gameplay.Features.Shop;
using _Project.Develop.Runtime.UI.Core;
using _Project.Develop.Runtime.Utilities.CoroutinesManagement;
using UnityEngine;

namespace _Project.Develop.Runtime.UI.Gameplay.Shop
{
    public class ShopPopupPresenter : PopupPresenterBase
    {
        public event Action NextRequested;

        private readonly ShopService _shopService;
        private readonly FieldPlacementService _fieldPlacementService;
        private readonly GameplayPresentersFactory _presentersFactory;
        private readonly ViewsFactory _viewsFactory;

        private readonly ShopPopupView _view;
        private readonly List<ShopTilePresenter> _shopTilePresenters = new List<ShopTilePresenter>();

        private ShopTilePresenter _selectedTilePresenter;

        public ShopPopupPresenter(
            ICoroutinesPerformer coroutinesPerformer,
            ShopService shopService,
            FieldPlacementService fieldPlacementService,
            GameplayPresentersFactory presentersFactory,
            ViewsFactory viewsFactory,
            ShopPopupView view) : base(coroutinesPerformer)
        {
            _shopService = shopService;
            _fieldPlacementService = fieldPlacementService;
            _presentersFactory = presentersFactory;
            _viewsFactory = viewsFactory;
            _view = view;
        }

        protected override PopupViewBase PopupView => _view;

        public override void Initialize()
        {
            base.Initialize();

            foreach (ShopItemConfig item in _shopService.Items)
            {
                ShopTileView shopTileView = _viewsFactory.Create<ShopTileView>(ViewIDs.ShopTile);
                _view.ShopTilesListView.Add(shopTileView);

                ShopTilePresenter shopTilePresenter = _presentersFactory.CreateShopTilePresenter(shopTileView, item);
                shopTilePresenter.Initialize();

                _shopTilePresenters.Add(shopTilePresenter);
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            DeselectCurrentTile();

            foreach (ShopTilePresenter shopTilePresenter in _shopTilePresenters)
            {
                _view.ShopTilesListView.Remove(shopTilePresenter.ShopTileView);
                _viewsFactory.Release(shopTilePresenter.ShopTileView);
                shopTilePresenter.Dispose();
            }

            _shopTilePresenters.Clear();
        }

        protected override void OnPreShow()
        {
            base.OnPreShow();

            foreach (ShopTilePresenter shopTilePresenter in _shopTilePresenters)
            {
                shopTilePresenter.Subscribe();
                shopTilePresenter.SelectRequested += OnTileSelectRequested;
            }

            _fieldPlacementService.PositionPicked += OnFieldPositionPicked;
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            _fieldPlacementService.PositionPicked -= OnFieldPositionPicked;
            DeselectCurrentTile();

            foreach (ShopTilePresenter shopTilePresenter in _shopTilePresenters)
            {
                shopTilePresenter.SelectRequested -= OnTileSelectRequested;
                shopTilePresenter.UnSubscribe();
            }

            NextRequested?.Invoke();
        }

        private void OnTileSelectRequested(ShopTilePresenter tilePresenter)
        {
            if (_shopService.CanBuy(tilePresenter.Item) == false)
                return;

            if (_shopService.NeedsFieldPlacement(tilePresenter.Item) == false)
            {
                _shopService.TryBuy(tilePresenter.Item);
                DeselectUnaffordableTile();
                return;
            }

            if (_selectedTilePresenter == tilePresenter)
            {
                DeselectCurrentTile();
                return;
            }

            SelectTile(tilePresenter);
        }

        private void OnFieldPositionPicked(Vector3 position)
        {
            if (_selectedTilePresenter == null)
                return;

            if (_shopService.TryBuyAt(_selectedTilePresenter.Item, position) == false)
                return;

            DeselectUnaffordableTile();
        }

        private void SelectTile(ShopTilePresenter tilePresenter)
        {
            DeselectCurrentTile();

            _selectedTilePresenter = tilePresenter;
            _selectedTilePresenter.Select();

            _fieldPlacementService.Begin();
        }

        private void DeselectUnaffordableTile()
        {
            if (_selectedTilePresenter == null)
                return;

            if (_shopService.CanBuy(_selectedTilePresenter.Item) == false)
                DeselectCurrentTile();
        }

        private void DeselectCurrentTile()
        {
            if (_selectedTilePresenter == null)
                return;

            _selectedTilePresenter.Deselect();
            _selectedTilePresenter = null;

            _fieldPlacementService.Cancel();
        }
    }
}
