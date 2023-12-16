using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.Obstacle;
using CodeBase.Gameplay.States;

namespace CodeBase.Gameplay.Services
{
    public class BlockHandler
    {
        private readonly ReleasedBlockTracker _releasedBlockTracker;
        private readonly LevelStateMachine _levelStateMachine;
        private readonly CollisionHandler _collisionHandler;

        public BlockHandler(
            ReleasedBlockTracker releasedBlockTracker,
            LevelStateMachine levelStateMachine,
            CollisionHandler collisionHandler
            )
        {
            _releasedBlockTracker = releasedBlockTracker;
            _levelStateMachine = levelStateMachine;
            _collisionHandler = collisionHandler;
        }

        public void HandleRelease(Block block) => 
            _releasedBlockTracker.StartTracking(block);

        public void HandleCollision(Block block, IObstacle obstacle) => 
            _collisionHandler.Handle(block, obstacle);

        public void HandleMiss() => 
            SwitchToFailState();

        private void SwitchToFailState() => 
            _levelStateMachine.Enter<LevelFailState>();
    }
}