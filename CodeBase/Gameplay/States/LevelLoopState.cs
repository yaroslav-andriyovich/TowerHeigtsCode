using CodeBase.Gameplay.HoistingRopeLogic;
using CodeBase.Gameplay.Services;
using CodeBase.Gameplay.Services.BlockBind;
using CodeBase.Gameplay.Services.BlockMiss;
using CodeBase.Infrastructure.States;
using UnityEngine.InputSystem;

namespace CodeBase.Gameplay.States
{
    public class LevelLoopState : ILevelState, IState
    {
        private readonly BlockBinder _blockBinder;
        private readonly CollisionObserver _collisionObserver;
        private readonly HoistingRope _hoistingRope;
        private readonly MissChecker _missChecker;
        private readonly BlockHandler _blockHandler;

        private InputActions.GameplayActions _gameplayInput;

        public LevelLoopState(
            InputActions inputActions,
            BlockBinder blockBinder,
            CollisionObserver collisionObserver,
            HoistingRope hoistingRope, 
            MissChecker missChecker,
            BlockHandler blockHandler
        )
        {
            _gameplayInput = inputActions.Gameplay;
            _blockBinder = blockBinder;
            _collisionObserver = collisionObserver;
            _hoistingRope = hoistingRope;
            _missChecker = missChecker;
            _blockHandler = blockHandler;
        }

        public void Enter()
        {
            _hoistingRope.OnReleased += _blockHandler.HandleRelease;
            _collisionObserver.OnCollision += _blockHandler.HandleCollision;
            _missChecker.OnMiss += _blockHandler.HandleMiss;

            EnableInput();
        }

        public void Exit()
        {
            _hoistingRope.OnReleased -= _blockHandler.HandleRelease;
            _collisionObserver.OnCollision -= _blockHandler.HandleCollision;
            _missChecker.OnMiss -= _blockHandler.HandleMiss;

            DisableInput();
        }

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