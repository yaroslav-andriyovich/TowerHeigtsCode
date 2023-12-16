using DG.Tweening;
using UnityEngine;

namespace CodeBase.Gameplay.BaseBlock.Animator
{
    public class BlockAnimator : MonoBehaviour
    {
        [SerializeField] private BlockAnimatorConfig _animatorConfig;

        private void OnDisable() => 
            Restore();

        public void Restore() => 
            transform.DOKill();

        public void Crash(float crashDirection)
        {
            DoJump(crashDirection);
            DoRotation(crashDirection);
        }
        
        public void Collapse(float collapseDirection) => 
            DoRotation(collapseDirection);

        private void DoJump(float crashDirection)
        {
            Vector3 position = transform.position;
            float x = position.x + crashDirection * _animatorConfig.crashEndPoint.x;
            float y = position.y + _animatorConfig.crashEndPoint.y;

            transform
                .DOJump(new Vector3(x, y, position.z), jumpPower: _animatorConfig.crashPower, numJumps: 1, duration: _animatorConfig.crashTime, snapping: false)
                .SetEase(_animatorConfig.crashEase);
        }

        private void DoRotation(float crashDirection)
        {
            Vector3 angle = Quaternion.AngleAxis(_animatorConfig.crashAngle, Vector3.forward * crashDirection).eulerAngles;

            transform
                .DORotate(angle, _animatorConfig.crashTime)
                .SetEase(_animatorConfig.crashEase);
        }
    }
}