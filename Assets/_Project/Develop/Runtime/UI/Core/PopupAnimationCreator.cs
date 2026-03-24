using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Develop.Runtime.UI.Core
{
    public class PopupAnimationCreator
    {
        public static Sequence CreateShowAnimation(
            CanvasGroup body, 
            Image antiClicker,
            PopupAnimationType animationType, 
            float antiClickerMaxAlpha)
        {
            switch (animationType)
            {
                case PopupAnimationType.None:
                    return DOTween.Sequence();

                case PopupAnimationType.Expand:
                    return DOTween.Sequence()
                        .Append(antiClicker
                            .DOFade(antiClickerMaxAlpha, 0.2f)
                            .From(0))
                        .Join(body.transform
                            .DOScale(1, 0.5f)
                            .From(0)
                            .SetEase(Ease.OutBack));

                case PopupAnimationType.Fade:
                    return DOTween.Sequence()
                        .Append(antiClicker
                            .DOFade(antiClickerMaxAlpha, 0.2f)
                            .From(0))
                        .Join(body
                            .DOFade(1, 0.3f)
                            .From(0));

                default:
                    throw new ArgumentException(nameof(animationType));
            }
        }

        public static Sequence CreateHideAnimation(
            CanvasGroup body, 
            Image antiClicker,
            PopupAnimationType animationType, 
            float antiClickerMaxAlpha)
        {
            return DOTween.Sequence();
        }
    }
}
