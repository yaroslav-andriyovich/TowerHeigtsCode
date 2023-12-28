using CodeBase.Gameplay.BlocksPool;
using CodeBase.Gameplay.CameraManagement;
using CodeBase.Gameplay.RopeManagement;
using CodeBase.Gameplay.TransformDescend;
using CodeBase.Infrastructure.States;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CodeBase.Gameplay.States
{
    public class LevelStartState : ILevelState, IState, ITickable
    {
        private readonly LevelStateMachine _stateMachine;
        private readonly IBlockPool _blockPool;
        private readonly RopeAttachment _ropeAttachment;
        private readonly ReleaseTimer _releaseTimer;
        private readonly RopeMovement _ropeMovement;
        private readonly TransformDescender _transformDescender;
        private readonly CameraBlur _cameraBlur;

        private InputActions.MenuActions _menuInput;

        public LevelStartState(
            LevelStateMachine stateMachine,
            InputActions inputActions, 
            IBlockPool blockPool,
            Rope rope,
            RopeAttachment ropeAttachment,
            ReleaseTimer releaseTimer,
            TransformDescender transformDescender,
            CameraBlur cameraBlur
        )
        {
            _menuInput = inputActions.Menu;
            _stateMachine = stateMachine;
            _blockPool = blockPool;
            _ropeMovement = rope.Movement;
            _ropeAttachment = ropeAttachment;
            _releaseTimer = releaseTimer;
            _transformDescender = transformDescender;
            _cameraBlur = cameraBlur;
        }

        public void Enter()
        {
            InitializeLevel();
            _menuInput.Enable();
        }

        public void Exit() => 
            _menuInput.Disable();

        public void Tick()
        {
            if (_menuInput.StartGame.WasPressedThisFrame())
                StartGame().Forget();
        }

        private void InitializeLevel()
        {
            _cameraBlur.FastBlur();
            _blockPool.CreatePool();
            _ropeMovement.ResetLocation();
            _transformDescender.DescendDefaultTransforms();
        }

        private async UniTaskVoid StartGame()
        {
            _menuInput.Disable();
            _ropeAttachment.AttachBlock();
            _cameraBlur.SmoothFocus();
            await _ropeMovement.Drop();
            _stateMachine.Enter<LevelLoopState>();
        }
    }
}