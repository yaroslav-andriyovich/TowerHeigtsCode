using CodeBase.Gameplay.BlocksPool;
using CodeBase.Gameplay.HoistingRopeLogic;
using CodeBase.Gameplay.Services.BlockBind;
using CodeBase.Gameplay.Services.BlockReleaseTimer;
using CodeBase.Gameplay.Services.TransformDescend;
using CodeBase.Infrastructure.States;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CodeBase.Gameplay.States
{
    public class LevelStartState : ILevelState, IState, ITickable
    {
        private readonly LevelStateMachine _stateMachine;
        private readonly IBlockPool _blockPool;
        private readonly BlockBinder _blockBinder;
        private readonly ReleaseTimerController _releaseTimerController;
        private readonly RopeMovement _ropeMovement;
        private readonly TransformDescender _transformDescender;

        private InputActions.MenuActions _menuInput;

        public LevelStartState(
            LevelStateMachine stateMachine,
            InputActions inputActions, 
            IBlockPool blockPool,
            HoistingRope hoistingRope,
            BlockBinder blockBinder,
            ReleaseTimerController releaseTimerController,
            TransformDescender transformDescender
        )
        {
            _menuInput = inputActions.Menu;
            _stateMachine = stateMachine;
            _blockPool = blockPool;
            _ropeMovement = hoistingRope.Movement;
            _blockBinder = blockBinder;
            _releaseTimerController = releaseTimerController;
            _transformDescender = transformDescender;
        }

        public void Enter()
        {
            Initialize();
            _menuInput.Enable();
        }

        public void Exit() => 
            _menuInput.Disable();

        public void Tick()
        {
            if (_menuInput.StartGame.WasPressedThisFrame())
                StartGame().Forget();
        }

        private void Initialize()
        {
            _blockPool.CreatePool();
            _ropeMovement.ResetLocation();
            _transformDescender.DescendDefaultTransforms();
        }

        private async UniTaskVoid StartGame()
        {
            _menuInput.Disable();
            _blockBinder.BindNext();
            await _ropeMovement.Drop();
            _releaseTimerController.Start();
            SwitchToLoopState();
        }

        private void SwitchToLoopState() => 
            _stateMachine.Enter<LevelLoopState>();
    }
}