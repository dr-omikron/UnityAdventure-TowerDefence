using _Project.Develop.Runtime.UI.Core;
using _Project.Develop.Runtime.Utilities.CoroutinesManagement;

namespace _Project.Develop.Runtime.UI.Gameplay.Shop
{
    public class ShopPopupPresenter : PopupPresenterBase
    {
        public ShopPopupPresenter(ICoroutinesPerformer coroutinesPerformer) 
            : base(coroutinesPerformer)
        {
        }

        protected override PopupViewBase PopupView { get; }
    }
}