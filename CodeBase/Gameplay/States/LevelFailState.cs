using CodeBase.Gameplay.BlockTracking;
using CodeBase.Gameplay.CameraManagement;
using CodeBase.Gameplay.RopeManagement;
using CodeBase.Infrastructure.States;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Gameplay.States
{
    public class LevelFailState : ILevelState, IState
    {
        private readonly BlockTracker _blockTracker;
        private readonly ReleaseTimer _releaseTimer;
        private readonly GameFlow _gameFlow;
        private readonly RopeMovement _ropeMovement;
        private readonly CameraBlur _cameraBlur;

        public LevelFailState(
            BlockTracker blockTracker,
            ReleaseTimer releaseTimer,
            GameFlow gameFlow,
            Rope rope,
            CameraBlur cameraBlur
        )
        {
            _blockTracker = blockTracker;
            _releaseTimer = releaseTimer;
            _ropeMovement = rope.Movement;
            _gameFlow = gameFlow;
            _cameraBlur = cameraBlur;
        }

        public void Enter()
        {
            _blockTracker.StopTracking();
            _releaseTimer.Stop();
            _gameFlow.Cleanup();

            _ropeMovement.Raise().Forget();
            _cameraBlur.SmoothBlur();
            Debug.Log("Game Over!");
        }

        public void Exit()
        {
        }
    }
}