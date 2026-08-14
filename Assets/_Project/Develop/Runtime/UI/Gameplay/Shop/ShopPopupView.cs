using _Project.Develop.Runtime.UI.Core;
using DG.Tweening;
using UnityEngine;

namespace _Project.Develop.Runtime.UI.Gameplay.Shop
{
    public class ShopPopupView : PopupViewBase
    {
        [SerializeField] private ShopTilesListView _shopTilesListView;

        public ShopTilesListView ShopTilesListView => _shopTilesListView;

        protected override void ModifyShowAnimation(Sequence sequence)
        {
            base.ModifyShowAnimation(sequence);

            foreach (ShopTileView shopTileView in _shopTilesListView.Elements)
            {
                sequence.Append(shopTileView.Show());
                sequence.AppendInterval(0.1f);
            }
        }
    }
}