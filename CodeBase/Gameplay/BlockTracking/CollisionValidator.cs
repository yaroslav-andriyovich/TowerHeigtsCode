using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.Combo;

namespace CodeBase.Gameplay.BlockTracking
{
    public class CollisionValidator
    {
        private readonly OffsetChecker _offsetChecker;
        private readonly ObstacleValidator _obstacleValidator;
        private readonly ComboSystem _comboSystem;

        public CollisionValidator(OffsetChecker offsetChecker, ObstacleValidator obstacleValidator, ComboSystem comboSystem)
        {
            _offsetChecker = offsetChecker;
            _obstacleValidator = obstacleValidator;
            _comboSystem = comboSystem;
        }

        public CollisionResult Validate(Block block, IObstacle obstacle)
        {
            OffsetCheckerResult offsetResult = CheckOffset(block, obstacle);

            CollisionResult result = new CollisionResult()
            {
                isSuccess = IsCorrectObstacle(obstacle) && offsetResult.isPermissible,
                isCombo = _comboSystem.CheckCombo(offsetResult.percent),
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