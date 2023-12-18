using System;
using CodeBase.Gameplay.BlockTracking;
using CodeBase.Gameplay.RopeManagement;
using CodeBase.Gameplay.Stability;
using CodeBase.Gameplay.States;

namespace CodeBase.Gameplay
{
    public class GameFlow : IDisposable
    {
        private readonly LevelStateMachine _levelStateMachine;
        private readonly CollisionHandler _collisionHandler;
        private readonly RopeAttachment _ropeAttachment;
        private readonly MissChecker _missChecker;
        private readonly TowerStability _towerStability;

        public GameFlow(
            LevelStateMachine levelStateMachine,
            CollisionHandler collisionHandler,
            RopeAttachment ropeAttachment,
            MissChecker missChecker,
            TowerStability towerStability
            )
        {
            _levelStateMachine = levelStateMachine;
            _collisionHandler = collisionHandler;
            _ropeAttachment = ropeAttachment;
            _missChecker = missChecker;
            _towerStability = towerStability;
            
            _collisionHandler.OnSuccessful += _ropeAttachment.AttachBlock;
            _collisionHandler.OnFail += FailGame;
            _missChecker.OnMiss += FailGame;
            _towerStability.OnCollapsed += FailGame;
        }

        public void Dispose() => 
            Cleanup();

        public void Cleanup()
        {
            _collisionHandler.OnSuccessful -= _ropeAttachment.AttachBlock;
            _collisionHandler.OnFail -= FailGame;
            _missChecker.OnMiss -= FailGame;
            _towerStability.OnCollapsed -= FailGame;
        }

        private void FailGame() => 
            _levelStateMachine.Enter<LevelFailState>();
    }
}