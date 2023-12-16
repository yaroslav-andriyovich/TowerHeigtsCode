using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CodeBase.UI
{
    [Serializable]
    public class UIIdlePanel
    {
        [SerializeField] private CanvasGroup _canvas;
        [SerializeField] private float _fadeAnimationTime;
        [Space]
        [SerializeField] private TMP_Text _playText;
        [SerializeField] private float _playTextAnimationTime;

        public void Clear()
        {
            _canvas.DOKill();
            _playText.DOKill();
        }

        public void Show()
        {
            _canvas.gameObject.SetActive(true);
            _canvas.DOFade(1f, 0f);

            AnimatePlayText();
        }

        public void Hide()
        {
            _canvas
                .DOFade(0f, _fadeAnimationTime)
                .OnComplete(() =>
                {
                    _canvas.gameObject.SetActive(false);
                });
        }

        private void AnimatePlayText() =>
            _playText.
                DOFade(0f, _playTextAnimationTime).
                SetLoops(-1, LoopType.Yoyo);
    }
}