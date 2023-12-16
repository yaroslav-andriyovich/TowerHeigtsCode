using UnityEngine;

namespace CodeBase.Gameplay.BaseBlock.Fall
{
    public class BlockHorizontalMover
    {
        private readonly Transform _transform;
        private readonly BlockFallConfig _fallConfig;
        
        private float _direction;
        private float _horizontalSpeed;

        public BlockHorizontalMover(Transform transform, BlockFallConfig fallConfig)
        {
            _transform = transform;
            _fallConfig = fallConfig;
        }

        public void InitializeMoving(float horizontalOffset)
        {
            _direction = Mathf.Sign(horizontalOffset);
            _horizontalSpeed = horizontalOffset;
        }

        public void Move(float deltaTime)
        {
            Vector3 acceleration = new Vector3(CalculateSpeed(deltaTime), 0f, 0f);
            
            _transform.position += acceleration;
        }
        
        public void Restore()
        {
            _horizontalSpeed = 0f;
            _direction = 0f;
        }

        private float CalculateSpeed(float deltaTime)
        {
            float direction = Mathf.Sign(_direction);
            float speedIncreaseValue = -direction * _fallConfig.horizontalGravity * deltaTime;

            _horizontalSpeed += speedIncreaseValue;

            if (direction > 0f)
                _horizontalSpeed = Mathf.Clamp(_horizontalSpeed, 0f, _fallConfig.horizontalMaxSpeed);
            else
                _horizontalSpeed = Mathf.Clamp(_horizontalSpeed, -_fallConfig.horizontalMaxSpeed, 0f);
                
            return _horizontalSpeed;
        }
    }
}