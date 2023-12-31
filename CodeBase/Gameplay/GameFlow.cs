using System;
using CodeBase.Gameplay.BlockTracking;
using CodeBase.Gameplay.RopeManagement;
using CodeBase.Gameplay.Stability;
using CodeBase.Gameplay.States;
using Zenject;

namespace CodeBase.Gameplay
{
    public class GameFlow : IInitializable, IDisposable
    {
        private readonly LevelStateMachine _levelStateMachine;
        private readonly CollisionHandler _collisionHandler;
        private readonly RopeAttachment _ropeAttachment;
        private readonly MissDetector _missDetector;
        private readonly TowerCollapse _towerCollapse;

        public GameFlow(
            LevelStateMachine levelStateMachine,
            CollisionHandler collisionHandler,
            RopeAttachment ropeAttachment,
            MissDetector missDetector,
            TowerCollapse towerCollapse
            )
        {
            _levelStateMachine = levelStateMachine;
            _collisionHandler = collisionHandler;
            _ropeAttachment = ropeAttachment;
            _missDetector = missDetector;
            _towerCollapse = towerCollapse;
        }

        public void Initialize()
        {
            _collisionHandler.OnGoodCollision += _ropeAttachment.AttachBlock;
            _collisionHandler.OnBadCollision += FailGame;
            _missDetector.OnMiss += FailGame;
            _towerCollapse.OnCollapsed += FailGame;
        }

        public void Dispose() => 
            Cleanup();

        public void Cleanup()
        {
            _collisionHandler.OnGoodCollision -= _ropeAttachment.AttachBlock;
            _collisionHandler.OnBadCollision -= FailGame;
            _missDetector.OnMiss -= FailGame;
            _towerCollapse.OnCollapsed -= FailGame;
        }

        private void FailGame() => 
            _levelStateMachine.Enter<LevelFailState>();
    }
}