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
        private const float NORMAL_SCALE = 1f;
        private const float SELECTED_SCALE = 1.1f;
        private const float SELECTION_ANIMATION_TIME = 0.1f;

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
        public void SetCost(string cost) => _costText.text = cost;

        public void SetSelected() => ScaleTo(SELECTED_SCALE);

        public void SetDeselected() => ScaleTo(NORMAL_SCALE);

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

        private void OnDestroy()
        {
            transform.DOKill();
        }

        private void ScaleTo(float scale)
        {
            transform.DOKill();

            transform
                .DOScale(scale, SELECTION_ANIMATION_TIME)
                .SetUpdate(true)
                .Play();
        }

        private void OnClick() => Clicked?.Invoke();
    }
}