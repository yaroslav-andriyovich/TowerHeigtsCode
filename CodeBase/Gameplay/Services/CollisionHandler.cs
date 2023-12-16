using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.Obstacle;
using CodeBase.Gameplay.Services.BlockBind;
using CodeBase.Gameplay.Services.BlockReleaseTimer;
using CodeBase.Gameplay.Services.Collision;
using CodeBase.Gameplay.Services.Combo;
using CodeBase.Gameplay.Stability;
using CodeBase.Gameplay.States;

namespace CodeBase.Gameplay.Services
{
    public class CollisionHandler
    {
        private readonly ReleasedBlockTracker _releasedBlockTracker;
        private readonly ReleaseTimerController _releaseTimerController;
        private readonly ObstacleValidator _obstacleValidator;
        private readonly OffsetChecker _offsetChecker;
        private readonly StabilityController _stabilityController;
        private readonly LandingController _landingController;
        private readonly LevelStateMachine _stateMachine;
        private readonly BlockBinder _blockBinder;
        private readonly ComboChecker _comboChecker;

        public CollisionHandler(
            ReleasedBlockTracker releasedBlockTracker,
            ReleaseTimerController releaseTimerController,
            ObstacleValidator obstacleValidator,
            OffsetChecker offsetChecker,
            StabilityController stabilityController,
            LandingController landingController,
            LevelStateMachine stateMachine,
            BlockBinder blockBinder,
            ComboChecker comboChecker
            )
        {
            _releasedBlockTracker = releasedBlockTracker;
            _releaseTimerController = releaseTimerController;
            _obstacleValidator = obstacleValidator;
            _offsetChecker = offsetChecker;
            _stabilityController = stabilityController;
            _landingController = landingController;
            _stateMachine = stateMachine;
            _blockBinder = blockBinder;
            _comboChecker = comboChecker;
        }

        public void Handle(Block block, IObstacle obstacle)
        {
            _releasedBlockTracker.StopTracking();
            _releaseTimerController.Stop();
            
            bool isCorrectObstacle = _obstacleValidator.IsCorrectObstacle(obstacle);
            OffsetCheckerResult offset = _offsetChecker.IsPermissibleOffset(obstacle, block.transform.position);

            if (!isCorrectObstacle || !offset.isPermissible)
            {
                FailGameByCrash(block, offset);
                return;
            }

            LandBlock(block, offset);
            
            if (!_stabilityController.IsStable)
                FailGameByTowerCollapse();
            else
                NextLoopStep();
        }
        
        private void FailGameByCrash(Block block, OffsetCheckerResult offset)
        {
            _landingController.LandWithCrash(block, offset.direction);
            SwitchToFailState();
        }

        private void SwitchToFailState() => 
            _stateMachine.Enter<LevelFailState>();

        private void LandBlock(Block block, OffsetCheckerResult offset)
        {
            bool isCombo = _comboChecker.CheckCombo(offset.percent);

            _landingController.Land(block, isCombo);
            _stabilityController.Change(offset.percent, isCombo);
        }

        private void FailGameByTowerCollapse() => 
            SwitchToFailState();

        private void NextLoopStep()
        {
            _blockBinder.BindNext();
            _releaseTimerController.Start();
        }
    }
}