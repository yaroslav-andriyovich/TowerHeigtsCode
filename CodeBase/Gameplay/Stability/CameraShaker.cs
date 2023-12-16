using DG.Tweening;
using UnityEngine;

namespace CodeBase.Gameplay.Stability
{
    public class CameraShaker : MonoBehaviour
    {
        [SerializeField] private float _duration;
        [SerializeField] private float _strength;

        private void OnDestroy() => 
            transform.DOKill();

        public void Shake() => 
            transform.DOShakePosition(_duration, _strength);
    }
}