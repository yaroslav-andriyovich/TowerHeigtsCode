using CodeBase.Extensions;
using UnityEngine;

namespace CodeBase.Gameplay.BaseBlock.Fall
{
    public class BlockFallController : MonoBehaviour
    {
        [SerializeField] private BlockFallConfig _fallConfig;
        [SerializeField] private BlockOffsetTracker _offsetTracker;

        private BlockHorizontalMover _horizontalMover;
        private BlockVerticalMover _verticalMover;
        private BlockRotationStabilizer _rotationStabilizer;

        private void Awake()
        {
            _horizontalMover = new BlockHorizontalMover(transform, _fallConfig);
            _verticalMover = new BlockVerticalMover(transform, _fallConfig);
            _rotationStabilizer = new BlockRotationStabilizer(transform, _fallConfig);
        }

        private void OnEnable() => 
            InitializeHorizontalMoving();

        private void OnDisable() => 
            Restore();

        private void Update()
        {
            if (Time.timeScale == 0f)
                return;

            float deltaTime = Time.deltaTime;

            _horizontalMover.Move(deltaTime);
            _verticalMover.Move(deltaTime);
            _rotationStabilizer.Stabilize(deltaTime);
        }

        public void ResetRotation() =>
            _rotationStabilizer.Reset();

        public void Restore()
        {
            this.DisableComponent();
            
            _horizontalMover.Restore();
            _verticalMover.Restore();

            _offsetTracker.EnableComponent();
        }

        private void InitializeHorizontalMoving()
        {
            _offsetTracker.DisableComponent();
            
            _horizontalMover.InitializeMoving(_offsetTracker.HorizontalOffset);
        }
    }
}