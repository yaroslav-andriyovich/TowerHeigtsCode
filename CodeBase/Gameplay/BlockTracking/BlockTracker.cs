using System;
using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.RopeManagement;
using Zenject;

namespace CodeBase.Gameplay.BlockTracking
{
    public class BlockTracker : IInitializable, IDisposable
    {
        private readonly Rope _rope;
        private readonly CollisionDetector _collisionDetector;
        private readonly MissDetector _missDetector;

        public BlockTracker(
            Rope rope, 
            CollisionDetector collisionDetector,
            MissDetector missDetector
            )
        {
            _rope = rope;
            _collisionDetector = collisionDetector;
            _missDetector = missDetector;
        }

        public void Initialize() => 
            _rope.OnReleased += OnBlockReleased;

        public void Dispose() => 
            _rope.OnReleased -= OnBlockReleased;

        public void StopTracking()
        {
            _collisionDetector.Cleanup();
            _missDetector.Stop();
        }

        private void OnBlockReleased(Block block)
        {
            _collisionDetector.Register(block);
            _missDetector.Run(block);
        }
    }
}