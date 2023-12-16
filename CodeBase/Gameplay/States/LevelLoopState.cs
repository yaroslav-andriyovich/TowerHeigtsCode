using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.HoistingRopeLogic;
using CodeBase.Gameplay.Obstacle;
using CodeBase.Gameplay.Services;
using CodeBase.Gameplay.Services.BlockBind;
using CodeBase.Gameplay.Services.BlockMiss;
using CodeBase.Gameplay.Services.BlockReleaseTimer;
using CodeBase.Gameplay.Services.Collision;
using CodeBase.Gameplay.Services.Combo;
using CodeBase.Gameplay.Stability;
using CodeBase.Infrastructure.States;
using UnityEngine.InputSystem;

namespace CodeBase.Gameplay.States
{
    public class LevelLoopState : ILevelState, IState
    {
        private readonly LevelStateMachine _stateMachine;
        private readonly BlockBinder _blockBinder;
        private readonly ReleaseTimerController _releaseTimerController;
        private readonly StabilityController _stabilityController;
        private readonly CollisionObserver _collisionObserver;
        private readonly HoistingRope _hoistingRope;
        private readonly MissChecker _missChecker;
        private readonly ObstacleValidator _obstacleValidator;
        private readonly LandingController _landingController;
        private readonly OffsetChecker _offsetChecker;
        private readonly ComboChecker _comboChecker;
        private readonly ReleasedBlockTracker _releasedBlockTracker;

        private InputActions.GameplayActions _gameplayInput;

        public LevelLoopState(
            LevelStateMachine stateMachine,
            InputActions inputActions,
            BlockBinder blockBinder,
            ReleaseTimerController releaseTimerController,
            StabilityController stabilityController,
            CollisionObserver collisionObserver,
            HoistingRope hoistingRope, 
            MissChecker missChecker, 
            ObstacleValidator obstacleValidator,
            LandingController landingController,
            OffsetChecker offsetChecker,
            ComboChecker comboChecker,
            ReleasedBlockTracker releasedBlockTracker
        )
        {
            _stateMachine = stateMachine;
            _gameplayInput = inputActions.Gameplay;
            _blockBinder = blockBinder;
            _releaseTimerController = releaseTimerController;
            _stabilityController = stabilityController;
            _collisionObserver = collisionObserver;

            _hoistingRope = hoistingRope;
            _missChecker = missChecker;
            _obstacleValidator = obstacleValidator;
            _landingController = landingController;
            _offsetChecker = offsetChecker;
            _comboChecker = comboChecker;
            _releasedBlockTracker = releasedBlockTracker;
        }

        public void Enter()
        {
            _hoistingRope.OnReleased += OnBlockReleased;
            _collisionObserver.OnCollision += OnBlockCollision;
            _missChecker.OnMiss += OnBlockMissed;

            EnableInput();
        }

        public void Exit()
        {
            _hoistingRope.OnReleased -= OnBlockReleased;
            _collisionObserver.OnCollision -= OnBlockCollision;
            _missChecker.OnMiss -= OnBlockMissed;

            DisableInput();
        }

        private void OnBlockReleased(Block block) => 
            _releasedBlockTracker.StartTracking(block);

        private void OnBlockCollision(Block block, IObstacle obstacle)
        {
            _releasedBlockTracker.StopTracking();
            _releaseTimerController.Stop();
            HandleBlockCollision(block, obstacle);
        }

        private void HandleBlockCollision(Block block, IObstacle obstacle)
        {
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

        private void SwitchToFailState() => 
            _stateMachine.Enter<LevelFailState>();

        private void OnBlockMissed() => 
            SwitchToFailState();

        private void EnableInput()
        {
            _gameplayInput.Tap.performed += OnPlayerTap;
            _gameplayInput.Enable();
        }

        private void DisableInput()
        {
            _gameplayInput.Tap.performed -= OnPlayerTap;
            _gameplayInput.Disable();
        }

        private void OnPlayerTap(InputAction.CallbackContext ctx)
        {
            if (IsPlayerInputBlocked())
                return;
            
            _blockBinder.Unbind();
        }

        private bool IsPlayerInputBlocked() => 
            !_blockBinder.IsCanUnbind;
    }
}