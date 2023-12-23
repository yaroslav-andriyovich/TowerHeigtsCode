using System;
using CodeBase.Gameplay.BlockTracking;
using CodeBase.Gameplay.RopeManagement;
using CodeBase.Gameplay.Stability;
using CodeBase.Gameplay.States;
using CodeBase.Gameplay.TowerManagement;
using Zenject;

namespace CodeBase.Gameplay
{
    public class GameFlow : IInitializable, IDisposable
    {
        private readonly LevelStateMachine _levelStateMachine;
        private readonly CollisionHandler _collisionHandler;
        private readonly RopeAttachment _ropeAttachment;
        private readonly MissDetector _missDetector;
        private readonly Tower _tower;
        private readonly CameraShaker _cameraShaker;

        public GameFlow(
            LevelStateMachine levelStateMachine,
            CollisionHandler collisionHandler,
            RopeAttachment ropeAttachment,
            MissDetector missDetector,
            Tower tower,
            CameraShaker cameraShaker
            )
        {
            _levelStateMachine = levelStateMachine;
            _collisionHandler = collisionHandler;
            _ropeAttachment = ropeAttachment;
            _missDetector = missDetector;
            _tower = tower;
            _cameraShaker = cameraShaker;
        }

        public void Initialize()
        {
            _collisionHandler.OnGoodCollision += _ropeAttachment.AttachBlock;
            _collisionHandler.OnBadCollision += FailGame;
            _missDetector.OnMiss += FailGame;
            _tower.OnCollapsed += FailGameByTowerCollapse;
        }

        public void Dispose() => 
            Cleanup();

        public void Cleanup()
        {
            _collisionHandler.OnGoodCollision -= _ropeAttachment.AttachBlock;
            _collisionHandler.OnBadCollision -= FailGame;
            _missDetector.OnMiss -= FailGame;
            _tower.OnCollapsed -= FailGameByTowerCollapse;
        }

        private void FailGame() => 
            _levelStateMachine.Enter<LevelFailState>();

        private void FailGameByTowerCollapse()
        {
            _cameraShaker.Shake();
            FailGame();
        }
    }
}