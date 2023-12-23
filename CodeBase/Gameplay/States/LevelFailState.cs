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
        private readonly BlockTracker _blockTracker;
        private readonly ReleaseTimer _releaseTimer;
        private readonly GameFlow _gameFlow;
        private readonly RopeMovement _ropeMovement;
        private readonly SoundPlayer _soundPlayer;

        public LevelFailState(
            BlockTracker blockTracker,
            ReleaseTimer releaseTimer,
            GameFlow gameFlow,
            Rope rope,
            SoundPlayer soundPlayer
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
            _blockTracker.StopTracking();
            _releaseTimer.Stop();
            _gameFlow.Cleanup();

            _ropeMovement.Raise().Forget();
            _soundPlayer.PlayBlockBounce();
            Debug.Log("Game Over!");
        }

        public void Exit()
        {
        }
    }
}