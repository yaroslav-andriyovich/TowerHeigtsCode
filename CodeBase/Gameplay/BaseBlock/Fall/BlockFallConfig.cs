using UnityEngine;

namespace CodeBase.Gameplay.BaseBlock.Fall
{
    [CreateAssetMenu(fileName = "BlockFallConfig", menuName = "Configs/Block/Falling", order = 1)]
    public class BlockFallConfig : ScriptableObject
    {
        [Header("Horizontal")]
        [SerializeField] private float _horizontalMaxSpeed;
        [SerializeField] private float _horizontalGravity;

        [Header("Vertical")]
        [SerializeField] private float _verticalGravity;
        [SerializeField] private float _startVerticalSpeed;
        [SerializeField] public float _maxVerticalSpeed;

        [Header("Rotation")]
        [SerializeField, Min(0f)] private float _rotationSpeed;

        public float horizontalMaxSpeed => _horizontalMaxSpeed;
        public float horizontalGravity => _horizontalGravity;
        
        public float verticalGravity => _verticalGravity;
        public float startVerticalSpeed => _startVerticalSpeed;
        public float maxVerticalSpeed => _maxVerticalSpeed;
        
        public float rotationSpeed => _rotationSpeed;
    }
}