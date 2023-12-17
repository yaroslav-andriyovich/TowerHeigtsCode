using CodeBase.Gameplay.HoistingRopeLogic;
using CodeBase.Gameplay.Services;
using CodeBase.Gameplay.Services.BlockBind;
using CodeBase.Gameplay.Services.BlockMiss;
using CodeBase.Infrastructure.States;
using Zenject;

namespace CodeBase.Gameplay.States
{
    public class LevelLoopState : ILevelState, IState, ITickable
    {
        private readonly BlockBinder _blockBinder;
        private readonly BlockCollisionDetector _collisionDetector;
        private readonly HoistingRope _hoistingRope;
        private readonly MissChecker _missChecker;
        private readonly BlockHandler _blockHandler;

        private InputActions.GameplayActions _gameplayInput;

        public LevelLoopState(
            InputActions inputActions,
            BlockBinder blockBinder,
            BlockCollisionDetector collisionDetector,
            HoistingRope hoistingRope, 
            MissChecker missChecker,
            BlockHandler blockHandler
        )
        {
            _gameplayInput = inputActions.Gameplay;
            _blockBinder = blockBinder;
            _collisionDetector = collisionDetector;
            _hoistingRope = hoistingRope;
            _missChecker = missChecker;
            _blockHandler = blockHandler;
        }

        public void Enter()
        {
            _hoistingRope.OnReleased += _blockHandler.HandleRelease;
            _collisionDetector.OnCollision += _blockHandler.HandleCollision;
            _missChecker.OnMiss += _blockHandler.HandleMiss;

            _gameplayInput.Enable();
        }

        public void Exit()
        {
            _hoistingRope.OnReleased -= _blockHandler.HandleRelease;
            _collisionDetector.OnCollision -= _blockHandler.HandleCollision;
            _missChecker.OnMiss -= _blockHandler.HandleMiss;

            _gameplayInput.Disable();
        }

        public void Tick()
        {
            if (_gameplayInput.Tap.WasPressedThisFrame())
            {
                if (IsPlayerInputBlocked())
                    return;
            
                _blockBinder.Unbind();
            }
        }

        private bool IsPlayerInputBlocked() => 
            !_blockBinder.IsCanUnbind;
    }
}