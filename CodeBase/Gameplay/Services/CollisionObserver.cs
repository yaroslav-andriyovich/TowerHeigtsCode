using System;
using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.Obstacle;

namespace CodeBase.Gameplay.Services
{
    public class CollisionObserver : IDisposable
    {
        public event Action<Block, IObstacle> OnCollision;
        public Block Block { get; private set; }
        public bool IsTracking => Block != null;

        public void Dispose() => 
            StopTracking();

        public void StartTracking(Block block)
        {
            if (Block != null)
                throw new InvalidOperationException("Already tracking!");
            
            Block = block;
            Block.OnCollision += OnBlockCollision;
        }

        public void StopTracking()
        {
            if (IsTracking)
            {
                Block.OnCollision -= OnBlockCollision;
                Block = null;
            }
        }

        private void OnBlockCollision(IObstacle obstacle) => 
            NotifyResult(obstacle);

        private void NotifyResult(IObstacle obstacle) => 
            OnCollision?.Invoke(Block, obstacle);
    }
}