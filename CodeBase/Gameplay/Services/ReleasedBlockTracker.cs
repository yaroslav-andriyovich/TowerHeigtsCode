using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.Services.BlockMiss;
using CodeBase.Gameplay.Services.Collision;

namespace CodeBase.Gameplay.Services
{
    public class ReleasedBlockTracker
    {
        private readonly CollisionObserver _collisionObserver;
        private readonly MissChecker _missChecker;
        private readonly ObstacleValidator _obstacleValidator;

        public ReleasedBlockTracker(
            CollisionObserver collisionObserver, 
            MissChecker missChecker,
            ObstacleValidator obstacleValidator
        )
        {
            _collisionObserver = collisionObserver;
            _missChecker = missChecker;
            _obstacleValidator = obstacleValidator;
        }

        public void StartTracking(Block block)
        {
            _collisionObserver.StartTracking(block);
            _missChecker.Run(block, _obstacleValidator.GetCorrect());
        }
        
        public void StopTracking()
        {
            _collisionObserver.StopTracking();
            _missChecker.Stop();
        }
    }
}