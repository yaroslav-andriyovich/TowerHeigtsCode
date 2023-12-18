using System;
using CodeBase.Gameplay.BaseBlock;
using UnityEngine;

namespace CodeBase.Gameplay.RopeManagement
{
    public class Rope : MonoBehaviour
    {
        [SerializeField] private Transform _anchorPoint;
        [SerializeField] private RopeMovement _movement;

        public event Action<Block> OnAttached;
        public event Action<Block> OnReleased;
        
        public bool HasBlock => _block != null;
        public RopeMovement Movement => _movement;

        private Block _block;

        public void AttachBlock(Block block)
        {
            if (HasBlock)
                throw new InvalidOperationException("Rope has a block!");
            
            _block = block;
            _block.transform.parent = _anchorPoint;
            _block.transform.localPosition = new Vector3(0f, -block.Height / 2f, 0f);
            _block.transform.localRotation = Quaternion.identity;
            
            OnAttached?.Invoke(_block);
        }

        public void ReleaseBlock()
        {
            if (_block == null)
                return;

            Block block = _block;
            block.EnableFalling();
            block.transform.parent = null;
            _block = null;

            OnReleased?.Invoke(block);
        }
    }
}