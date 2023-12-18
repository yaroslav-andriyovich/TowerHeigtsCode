using CodeBase.Gameplay.TowerManagement;

namespace CodeBase.Gameplay.BlockTracking
{
    public class ObstacleValidator
    {
        private readonly Tower _tower;
        
        public ObstacleValidator(Tower tower) => 
            _tower = tower;

        public bool IsCorrectObstacle(IObstacle obstacle) =>
            IsObstacleExist(obstacle) && IsObstacleEqualsCorrect(obstacle);

        public IObstacle GetCorrect() =>
            !_tower.IsEmpty
                ? _tower.GetHighestBlock()
                : _tower.GetFoundation();

        private bool IsObstacleExist(IObstacle obstacle) => 
            obstacle != null;

        private bool IsObstacleEqualsCorrect(IObstacle obstacle) => 
            obstacle == GetCorrect();
    }
}