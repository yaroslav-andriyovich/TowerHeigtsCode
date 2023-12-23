using UnityEngine;

namespace CodeBase.Gameplay.BaseBlock.Fall
{
    [CreateAssetMenu(fileName = "BlockFallConfig", menuName = "Configs/Block/Falling", order = 1)]
    public class BlockFallConfig : ScriptableObject
    {
        [Header("Horizontal")]
        public float horizontalMaxSpeed = 0.025f;
        public float horizontalGravity = 0.012f;
        
        [Header("Vertical")]
        public float verticalGravity = -0.225f;
        public float startVerticalSpeed = -0.015f;
        public float maxVerticalSpeed = -0.12f;
        
        [Header("Rotation")]
        public float rotationSpeed = 3f;
    }
}