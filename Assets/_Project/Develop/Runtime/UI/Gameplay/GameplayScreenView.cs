using _Project.Develop.Runtime.UI.CommonViews;
using _Project.Develop.Runtime.UI.Core;
using UnityEngine;

namespace _Project.Develop.Runtime.UI.Gameplay
{
    public class GameplayScreenView : MonoBehaviour, IView
    {
        [field: SerializeField] public IconTextListView WalletView { get; private set; }
        [field: SerializeField] public IconTextView StagesView { get; private set; }
        [field: SerializeField] public Transform EntitiesHealthDisplay { get; private set; }
        [field: SerializeField] public Transform ReloadBarContainer { get; private set; }
    }
}
