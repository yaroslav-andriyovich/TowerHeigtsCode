using UnityEngine;

namespace CodeBase.Gameplay.BaseBlock.Fall
{
    public class BlockVerticalMover
    {
        private readonly Transform _transform;
        private readonly BlockFallConfig _fallConfig;
        
        private float _verticalSpeed;

        public BlockVerticalMover(Transform transform, BlockFallConfig fallConfig)
        {
            _transform = transform;
            _fallConfig = fallConfig;
        }

        public void Move(float deltaTime)
        {
            Vector3 acceleration = new Vector3(0f, CalculateSpeed(deltaTime), 0f);
            
            _transform.position += acceleration;
        }
        
        public void Restore() => 
            _verticalSpeed = _fallConfig.startVerticalSpeed;

        private float CalculateSpeed(float deltaTime)
        {
            float increasedSpeed = _verticalSpeed + _fallConfig.verticalGravity * deltaTime;
            
            _verticalSpeed = Mathf.Clamp(increasedSpeed, _fallConfig.maxVerticalSpeed, 0);

            return _verticalSpeed;
        }
    }
}