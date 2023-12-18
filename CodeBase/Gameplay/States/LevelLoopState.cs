using CodeBase.Gameplay.RopeManagement;
using CodeBase.Infrastructure.States;
using Zenject;

namespace CodeBase.Gameplay.States
{
    public class LevelLoopState : ILevelState, IState, ITickable
    {
        private readonly Rope _rope;

        private InputActions.GameplayActions _gameplayInput;

        public LevelLoopState(
            Rope rope,
            InputActions inputActions
        )
        {
            _rope = rope;
            _gameplayInput = inputActions.Gameplay;
        }

        public void Enter() => 
            _gameplayInput.Enable();

        public void Exit() => 
            _gameplayInput.Disable();

        public void Tick()
        {
            if (_gameplayInput.Tap.WasPressedThisFrame() && _rope.HasBlock)
                _rope.ReleaseBlock();
        }
    }
}