using CodeBase.Extensions;
using CodeBase.Gameplay.BaseBlock.Animator;
using CodeBase.Gameplay.BaseBlock.Collision;
using CodeBase.Gameplay.BaseBlock.Fall;
using UnityEngine;

namespace CodeBase.Gameplay.BaseBlock
{
    public class StandardBlock : Block
    {
        [SerializeField] private BlockFallController _fallController;
        [SerializeField] private BlockCollisionDetector _collisionDetector;
        [SerializeField] private BlockAnimator _animator;
        [Space]
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private AudioSource _hitAudio;

        private void Awake() => 
            _collisionDetector.OnCollision += InvokeOnCollision;

        private void OnDestroy() => 
            _collisionDetector.OnCollision -= InvokeOnCollision;

        public override void Restore()
        {
            _fallController.Restore();
            _collisionDetector.EnableComponent();
            _animator.Restore();
        }

        public override void EnableFalling() => 
            _fallController.EnableComponent();

        public override void DisableFalling() => 
            _fallController.DisableComponent();

        public override void Ground()
        {
            _collisionDetector.DisableComponent();
            DisableFalling();

            _particle.Play();
            _hitAudio.Play();
        }

        public override void Crash(float offsetDirection)
        {
            _collisionDetector.DisableComponent();
            DisableFalling();
            _animator.Crash(offsetDirection);
        }
        
        public override void Collapse(float collapseDirection)
        {
            _collisionDetector.DisableComponent();
            EnableFalling();
            _animator.Collapse(collapseDirection);
        }
    }
}