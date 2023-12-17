using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.Services.BlockMiss;
using CodeBase.Gameplay.Services.Collision;

namespace CodeBase.Gameplay.Services
{
    public class ReleasedBlockTracker
    {
        private readonly BlockCollisionDetector _collisionDetector;
        private readonly MissChecker _missChecker;
        private readonly ObstacleValidator _obstacleValidator;

        public ReleasedBlockTracker(
            BlockCollisionDetector collisionDetector, 
            MissChecker missChecker,
            ObstacleValidator obstacleValidator
        )
        {
            _collisionDetector = collisionDetector;
            _missChecker = missChecker;
            _obstacleValidator = obstacleValidator;
        }

        public void StartTracking(Block block)
        {
            _collisionDetector.StartDetect(block);
            _missChecker.Run(block, _obstacleValidator.GetCorrect());
        }
        
        public void StopTracking()
        {
            _collisionDetector.StopDetect();
            _missChecker.Stop();
        }
    }
}