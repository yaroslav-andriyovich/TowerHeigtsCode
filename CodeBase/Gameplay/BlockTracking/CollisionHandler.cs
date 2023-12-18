using System;
using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.Combo;
using CodeBase.Gameplay.RopeManagement;
using CodeBase.Gameplay.Stability;

namespace CodeBase.Gameplay.BlockTracking
{
    public class CollisionHandler : IDisposable
    {
        public event Action OnSuccessful;
        public event Action OnFail;
        
        private readonly BlockTracker _blockTracker;
        private readonly ReleaseTimer _releaseTimer;
        private readonly ObstacleValidator _obstacleValidator;
        private readonly OffsetChecker _offsetChecker;
        private readonly TowerStability _towerStability;
        private readonly BlockLanding _blockLanding;
        private readonly ComboSystem _comboSystem;
        private readonly CollisionDetector _collisionDetector;

        public CollisionHandler(
            BlockTracker blockTracker,
            ReleaseTimer releaseTimer,
            ObstacleValidator obstacleValidator,
            OffsetChecker offsetChecker,
            TowerStability towerStability,
            BlockLanding blockLanding,
            ComboSystem comboSystem,
            CollisionDetector collisionDetector
            )
        {
            _blockTracker = blockTracker;
            _releaseTimer = releaseTimer;
            _obstacleValidator = obstacleValidator;
            _offsetChecker = offsetChecker;
            _towerStability = towerStability;
            _blockLanding = blockLanding;
            _comboSystem = comboSystem;
            _collisionDetector = collisionDetector;

            _collisionDetector.OnCollision += HandleCollision;
        }

        public void Dispose() => 
            _collisionDetector.OnCollision -= HandleCollision;

        private void HandleCollision(Block block, IObstacle obstacle)
        {
            _blockTracker.StopTracking();
            _releaseTimer.Stop();
            
            bool isCorrectObstacle = _obstacleValidator.IsCorrectObstacle(obstacle);
            OffsetCheckerResult offset = _offsetChecker.IsPermissibleOffset(obstacle, block.transform.position);

            if (!isCorrectObstacle || !offset.isPermissible)
            {
                _blockLanding.LandWithCrash(block, offset.direction);
                OnFail?.Invoke();
                return;
            }

            LandBlock(block, offset);
            OnSuccessful?.Invoke();
        }

        private void LandBlock(Block block, OffsetCheckerResult offset)
        {
            bool isCombo = _comboSystem.CheckCombo(offset.percent);

            _blockLanding.Land(block, isCombo);
            _towerStability.Change(offset.percent, isCombo);
        }
    }
}