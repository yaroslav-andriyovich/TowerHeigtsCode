using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace CodeBase.Gameplay.CameraManagement
{
    [RequireComponent(typeof(PostProcessVolume))]
    public class CameraBlur : MonoBehaviour
    {
        [SerializeField, UnityEngine.Min(0.1f)] private float _minFocusDistance = 0.1f;
        [SerializeField, UnityEngine.Min(0.1f)] private float _maxFocusDistance = 10f;
        [SerializeField, UnityEngine.Min(0f)] private float _animationDuration = 1f;
        [SerializeField] private Ease _animationEase;
        
        private PostProcessVolume _postProcessVolume;
        private DepthOfField _depthOfField;
        private Tween _animationTween;

        private void Start()
        {
            _postProcessVolume = GetComponent<PostProcessVolume>();
            _postProcessVolume.profile.TryGetSettings(out _depthOfField);
        }

        private void OnDestroy() => 
            _animationTween?.Kill();

        public void FastBlur() => 
            _depthOfField.focusDistance.value = _minFocusDistance;

        public void FastFocus() => 
            _depthOfField.focusDistance.value = _maxFocusDistance;

        public void SmoothBlur() => 
            ChangeFocus(_minFocusDistance);

        public void SmoothFocus() => 
            ChangeFocus(_maxFocusDistance);

        private void ChangeFocus(float focusDistance)
        {
            _animationTween = DOTween.To(
                    () => _depthOfField.focusDistance.value,
                    x => _depthOfField.focusDistance.value = x,
                    endValue: focusDistance,
                    duration: _animationDuration
                )
                .SetEase(_animationEase);
        }
    }
}