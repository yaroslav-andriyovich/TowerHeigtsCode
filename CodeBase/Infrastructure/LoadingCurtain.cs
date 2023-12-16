using CodeBase.Extensions;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class LoadingCurtain : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _curtain;
        [SerializeField, Min(0f)] private float _animationTime;

        private void Awake() => 
            DontDestroyOnLoad(this);

        private void OnDestroy() => 
            _curtain.DOKill();

        public async UniTask Show()
        {
            gameObject.MakeActive();
            await ChangeVisibility(true);
        }

        public async UniTask Hide()
        {
            await ChangeVisibility(false);
            gameObject.MakeInactive();
        }

        private async UniTask ChangeVisibility(bool state) => 
            await _curtain
                .DOFade(state ? 1f : 0f, _animationTime)
                .AsyncWaitForCompletion();
    }
}