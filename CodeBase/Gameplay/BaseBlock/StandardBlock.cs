using CodeBase.Extensions;
using CodeBase.Gameplay.BaseBlock.Animator;
using CodeBase.Gameplay.BaseBlock.Fall;
using UnityEngine;

namespace CodeBase.Gameplay.BaseBlock
{
    public class StandardBlock : Block
    {
        [SerializeField] private BlockFallController _fallController;
        [SerializeField] private BlockAnimator _animator;
        [Space]
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private AudioSource _hitAudio;

        public override void Restore()
        {
            _fallController.Restore();
            _animator.Restore();
        }

        public override void EnableFalling() => 
            _fallController.EnableComponent();

        public override void DisableFalling() => 
            _fallController.DisableComponent();

        public override void Ground()
        {
            DisableFalling();

            _particle.Play();
            _hitAudio.Play();
        }

        public override void Crash(float offsetDirection)
        {
            DisableFalling();
            _animator.Crash(offsetDirection);
        }
        
        public override void Collapse(float collapseDirection)
        {
            EnableFalling();
            _animator.Collapse(collapseDirection);
        }
    }
}