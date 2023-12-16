using UnityEngine;

namespace CodeBase.Gameplay.BaseBlock.Fall
{
    public class BlockRotationStabilizer
    {
        private readonly Transform _transform;
        private readonly BlockFallConfig _fallConfig;

        public BlockRotationStabilizer(Transform transform, BlockFallConfig fallConfig)
        {
            _transform = transform;
            _fallConfig = fallConfig;
        }
        
        public void Reset() =>
            _transform.rotation = Quaternion.identity;

        public void Stabilize(float deltaTime)
        {
            Quaternion currentRotation = _transform.rotation;
            float tiltAngle = Mathf.Atan2(currentRotation.z, currentRotation.w) * 2f;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, -tiltAngle);
            
            _transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _fallConfig.rotationSpeed * deltaTime);
        }
    }
}