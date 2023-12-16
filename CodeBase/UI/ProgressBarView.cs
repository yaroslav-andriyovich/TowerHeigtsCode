using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public abstract class ProgressBarView : MonoBehaviour
    {
        [SerializeField] protected Image _progressImage;
        [SerializeField] protected Color _maxProgressColor;
        [SerializeField] protected Color _minProgressColor;

        protected float _progress;

        public virtual void SetProgress(float current, float max)
        {
            CalculateProgress(current, max);
            FillImage();
            ChangeColor();
        }

        protected virtual void CalculateProgress(float current, float max) => 
            _progress = 1f - current / max;

        protected virtual void FillImage() => 
            _progressImage.fillAmount = _progress;

        protected virtual void ChangeColor() => 
            _progressImage.color = Color.Lerp(_minProgressColor, _maxProgressColor, _progress);
    }
}