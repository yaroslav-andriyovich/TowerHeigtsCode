using CodeBase.Gameplay.BlockTracking;
using CodeBase.Gameplay.RopeManagement;
using CodeBase.Infrastructure.States;
using CodeBase.Sounds;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Gameplay.States
{
    public class LevelFailState : ILevelState, IState
    {
        private readonly SoundPlayer _soundPlayer;
        private readonly BlockTracker _blockTracker;
        private readonly ReleaseTimer _releaseTimer;
        private readonly RopeMovement _ropeMovement;
        private readonly GameFlow _gameFlow;

        public LevelFailState(
            SoundPlayer soundPlayer,
            BlockTracker blockTracker,
            ReleaseTimer releaseTimer,
            Rope rope,
            GameFlow gameFlow
        )
        {
            _soundPlayer = soundPlayer;
            _blockTracker = blockTracker;
            _releaseTimer = releaseTimer;
            _ropeMovement = rope.Movement;
            _gameFlow = gameFlow;
        }

        public void Enter()
        {
            _gameFlow.Cleanup();
            _blockTracker.StopTracking();
            _releaseTimer.Stop();

            _ropeMovement.Raise().Forget();
            _soundPlayer.PlayGameOver();
            Debug.Log("Game Over!");
        }

        public void Exit()
        {
        }
    }
}