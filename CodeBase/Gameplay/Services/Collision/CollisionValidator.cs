using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.Obstacle;
using CodeBase.Gameplay.Services.Combo;

namespace CodeBase.Gameplay.Services.Collision
{
    public class CollisionValidator
    {
        private readonly OffsetChecker _offsetChecker;
        private readonly ObstacleValidator _obstacleValidator;
        private readonly ComboChecker _comboChecker;

        public CollisionValidator(OffsetChecker offsetChecker, ObstacleValidator obstacleValidator, ComboChecker comboChecker)
        {
            _offsetChecker = offsetChecker;
            _obstacleValidator = obstacleValidator;
            _comboChecker = comboChecker;
        }

        public CollisionResult Validate(Block block, IObstacle obstacle)
        {
            OffsetCheckerResult offsetResult = CheckOffset(block, obstacle);

            CollisionResult result = new CollisionResult()
            {
                isSuccess = IsCorrectObstacle(obstacle) && offsetResult.isPermissible,
                isCombo = _comboChecker.CheckCombo(offsetResult.percent),
                block = block,
                obstacle = obstacle,
                offset = offsetResult
            };
            
            return result;
        }

        private bool IsCorrectObstacle(IObstacle obstacle) => 
            _obstacleValidator.IsCorrectObstacle(obstacle);

        private OffsetCheckerResult CheckOffset(Block block, IObstacle obstacle) => 
            _offsetChecker.IsPermissibleOffset(obstacle, block.transform.position);
    }

    public struct CollisionResult
    {
        public bool isSuccess;
        public bool isCombo;
        public Block block;
        public IObstacle obstacle;
        public OffsetCheckerResult offset;
    }
}