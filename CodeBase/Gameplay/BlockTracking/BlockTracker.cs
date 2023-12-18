using System;
using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.RopeManagement;

namespace CodeBase.Gameplay.BlockTracking
{
    public class BlockTracker : IDisposable
    {
        private readonly Rope _rope;
        private readonly CollisionDetector _collisionDetector;
        private readonly MissChecker _missChecker;

        public BlockTracker(
            Rope rope, 
            CollisionDetector collisionDetector,
            MissChecker missChecker
            )
        {
            _rope = rope;
            _collisionDetector = collisionDetector;
            _missChecker = missChecker;
            
            _rope.OnReleased += OnBlockReleased;
        }

        public void Dispose() => 
            _rope.OnReleased -= OnBlockReleased;

        public void StopTracking()
        {
            _collisionDetector.Cleanup();
            _missChecker.Stop();
        }

        private void OnBlockReleased(Block block)
        {
            _collisionDetector.Register(block);
            _missChecker.Run(block);
        }
    }
}