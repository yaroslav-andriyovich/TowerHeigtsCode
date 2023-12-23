using System;
using CodeBase.Gameplay.BaseBlock;
using Zenject;

namespace CodeBase.Gameplay.BlockTracking
{
    public class CollisionHandler : IInitializable, IDisposable
    {
        public event Action OnGoodCollision;
        public event Action OnBadCollision;
        
        private readonly BlockTracker _blockTracker;
        private readonly ObstacleValidator _obstacleValidator;
        private readonly OffsetChecker _offsetChecker;
        private readonly BlockLanding _blockLanding;
        private readonly CollisionDetector _collisionDetector;

        public CollisionHandler(
            BlockTracker blockTracker,
            ObstacleValidator obstacleValidator,
            OffsetChecker offsetChecker,
            BlockLanding blockLanding,
            CollisionDetector collisionDetector
            )
        {
            _blockTracker = blockTracker;
            _obstacleValidator = obstacleValidator;
            _offsetChecker = offsetChecker;
            _blockLanding = blockLanding;
            _collisionDetector = collisionDetector;
        }
        
        public void Initialize() => 
            _collisionDetector.OnCollision += Handle;

        public void Dispose() => 
            _collisionDetector.OnCollision -= Handle;

        private void Handle(Block block, IObstacle obstacle)
        {
            _blockTracker.StopTracking();
            
            CollisionOffset collisionOffset = _offsetChecker.CalculateOffset(obstacle, block);

            if (IsIncorrectObstacle(obstacle) || !collisionOffset.isAllowable)
                LandBlockWithCrash(block, collisionOffset);
            else
                LandBlockOnTower(block, collisionOffset);
        }

        private bool IsIncorrectObstacle(IObstacle obstacle) => 
            !_obstacleValidator.IsCorrectObstacle(obstacle);

        private void LandBlockWithCrash(Block block, CollisionOffset collisionOffset)
        {
            _blockLanding.BounceToSide(block, collisionOffset.direction);
            OnBadCollision?.Invoke();
        }

        private void LandBlockOnTower(Block block, CollisionOffset collisionOffset)
        {
            _blockLanding.LandOnTower(block, collisionOffset);
            OnGoodCollision?.Invoke();
        }
    }
}