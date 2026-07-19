using System;
using _Project.Develop.Runtime.UI.CommonViews;
using _Project.Develop.Runtime.UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuScreenView : MonoBehaviour, IView
    {
        public event Action PlayGameButtonClicked;

        [field:SerializeField] public IconTextListView WalletView { get; private set; }
        [SerializeField] private Button _playGameButton;

        private void OnEnable()
        {
            _playGameButton.onClick.AddListener(OnPlayGameButtonClicked);
        }

        private void OnDisable()
        {
            _playGameButton.onClick.RemoveListener(OnPlayGameButtonClicked);
        }

        private void OnPlayGameButtonClicked() => PlayGameButtonClicked?.Invoke();
    }
}
