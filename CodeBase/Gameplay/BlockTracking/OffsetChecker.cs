using CodeBase.Data.Level;
using CodeBase.Gameplay.BaseBlock;
using CodeBase.Services.StaticData;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.BlockTracking
{
    public class OffsetChecker : IInitializable
    {
        private readonly IStaticDataService _staticDataService;

        private OffsetCheckerData _config;

        public OffsetChecker(IStaticDataService staticDataService) => 
            _staticDataService = staticDataService;

        public void Initialize() => 
            _config = _staticDataService.ForCurrentMode().OffsetCheckerData;

        public CollisionOffset CalculateOffset(IObstacle obstacle, Block block)
        {
            float offset = GetObstacleOffset(obstacle, block.transform.position);
            float percentOffset = GetObstaclePercentOffset(obstacle, offset);

            return new CollisionOffset()
            {
                isAllowable = IsAllowable(percentOffset),
                offsetValue = offset,
                percent = percentOffset,
                direction = GetOffsetDirection(offset)
            };
        }

        public float GetObstacleOffset(IObstacle obstacle, Vector3 blockPosition)
        {
            Vector3 offsetFromObstacleCenter = blockPosition - obstacle.transform.position;
            Vector3 correctedOffset = CorrectOffsetFromLocalSpace(obstacle.transform, offsetFromObstacleCenter);

            return correctedOffset.x;
        }

        private Vector3 CorrectOffsetFromLocalSpace(Transform obstacle, Vector3 blockOffset) => 
            obstacle.InverseTransformVector(blockOffset);

        private float GetObstaclePercentOffset(IObstacle obstacle, float obstacleOffset)
        {
            float absoluteOffset = Mathf.Abs(obstacleOffset);
            float percentOffset = absoluteOffset / obstacle.Width;

            return percentOffset;
        }

        private bool IsAllowable(float percentOffset) => 
            percentOffset <= _config.maxOffsetPercent;

        public float GetOffsetDirection(float offset) => 
            Mathf.Sign(offset);
    }
    
    public struct CollisionOffset
    {
        public bool isAllowable;
        public float offsetValue;
        public float percent;
        public float direction;
    }
}