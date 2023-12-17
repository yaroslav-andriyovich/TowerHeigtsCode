using CodeBase.Gameplay.HoistingRopeLogic;
using CodeBase.Gameplay.Services;
using CodeBase.Gameplay.Services.BlockMiss;
using CodeBase.Gameplay.Services.BlockReleaseTimer;
using CodeBase.Infrastructure.States;
using CodeBase.Sounds;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Gameplay.States
{
    public class LevelFailState : ILevelState, IState
    {
        private readonly SoundPlayer _soundPlayer;
        private readonly MissChecker _missChecker;
        private readonly ReleaseTimerController _releaseTimerController;
        private readonly BlockCollisionDetector _collisionDetector;
        private readonly RopeMovement _ropeMovement;

        public LevelFailState(
            SoundPlayer soundPlayer, 
            MissChecker missChecker,
            ReleaseTimerController releaseTimerController,
            HoistingRope hoistingRope,
            BlockCollisionDetector collisionDetector
        )
        {
            _soundPlayer = soundPlayer;
            _missChecker = missChecker;
            _releaseTimerController = releaseTimerController;
            _collisionDetector = collisionDetector;
            _ropeMovement = hoistingRope.Movement;
        }

        public void Enter()
        {
            _collisionDetector.StopDetect();
            _missChecker.Stop();
            _releaseTimerController.Stop();

            _ropeMovement.Raise().Forget();
            _soundPlayer.PlayGameOver();
            Debug.Log("Game Over!");
        }

        public void Exit()
        {
        }
    }
}