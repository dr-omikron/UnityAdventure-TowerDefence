using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Develop.Runtime.UI.Core
{
    public abstract class PopupViewBase : MonoBehaviour, IShowableView
    {
        public event Action CloseRequested;

        [SerializeField] private CanvasGroup _mainGroup;
        [SerializeField] private Image _antiClicker;
        [SerializeField] private CanvasGroup _body;
        [SerializeField] private PopupAnimationType _animationType;

        private Tween _currentAnimation;
        private float _antiClickerDefaultAlpha;

        private void Awake()
        {
            _antiClickerDefaultAlpha = _antiClicker.color.a;
            _mainGroup.alpha = 0;
        }

        public void OnCloseButtonClicked() => CloseRequested?.Invoke();

        public Tween Show()
        {
            KillCurrentAnimation();

            OnPreShow();

            _mainGroup.alpha = 1;

            Sequence sequence = PopupAnimationCreator.CreateShowAnimation(
                _body, 
                _antiClicker, 
                _animationType, 
                _antiClickerDefaultAlpha);

            ModifyShowAnimation(sequence);
            sequence.OnComplete(OnPostShow);

            return _currentAnimation = sequence.SetUpdate(true).Play();
        }

        public Tween Hide()
        {
            KillCurrentAnimation();

            OnPreHide();

            Sequence sequence = PopupAnimationCreator.CreateHideAnimation(_body, 
                _antiClicker, 
                _animationType, 
                _antiClickerDefaultAlpha);

            ModifyHideAnimation(sequence);
            sequence.OnComplete(OnPostHide);

            return _currentAnimation = sequence.SetUpdate(true).Play();
        }

        protected virtual void ModifyShowAnimation(Sequence sequence) { }

        protected virtual void ModifyHideAnimation(Sequence sequence) { }

        protected virtual void OnPreShow() { }

        protected virtual void OnPostShow() { }

        protected virtual void OnPreHide() { }

        protected virtual void OnPostHide() { }

        private void OnDestroy() => KillCurrentAnimation();

        private void KillCurrentAnimation()
        {
            if (_currentAnimation != null)
                _currentAnimation.Kill();
        }
    }
}
