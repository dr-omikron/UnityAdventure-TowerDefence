using System;
using _Project.Develop.Runtime.UI.Core;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Develop.Runtime.UI.Gameplay.Shop
{
    public class ShopTileView : MonoBehaviour, IShowableView
    {
        public event Action Clicked;

        [SerializeField] private Image _background;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _costText;
        [SerializeField] private Button _button;

        [SerializeField] private Color _activeColor;
        [SerializeField] private Color _blockedColor;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        public void SetBlock() => _background.color = _blockedColor;
        public void SetActive() => _background.color = _activeColor;
        public void SetIcon(Sprite icon) => _icon.sprite = icon;
        public void SetTitle(string title) => _titleText.text = title;

        public Tween Show()
        {
            transform.DOKill();

            return transform
                .DOScale(1f, 0.1f)
                .From(0)
                .SetUpdate(true)
                .Play();
        }

        public Tween Hide()
        {
            transform.DOKill();
            return DOTween.Sequence();
        }

        private void OnClick() => Clicked?.Invoke();
    }
}