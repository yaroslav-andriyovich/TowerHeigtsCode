using CodeBase.Data.Level;
using CodeBase.Gameplay.Obstacle;
using CodeBase.Services.StaticData;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Services.Collision
{
    public class OffsetChecker : IInitializable
    {
        private readonly IStaticDataService _staticDataService;

        private float _maxOffsetPercent;

        public OffsetChecker(IStaticDataService staticDataService) => 
            _staticDataService = staticDataService;

        public void Initialize()
        {
            OffsetCheckerData config = _staticDataService.ForCurrentMode().OffsetCheckerData;
            _maxOffsetPercent = config.maxOffsetPercent;
        }

        public OffsetCheckerResult IsPermissibleOffset(IObstacle obstacle, Vector3 blockPosition)
        {
            float offset = GetObstacleOffset(obstacle, blockPosition);
            float percentOffset = GetObstaclePercentOffset(obstacle, offset);

            return new OffsetCheckerResult()
            {
                isPermissible = IsPermissible(percentOffset),
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

        private bool IsPermissible(float percentOffset) => 
            percentOffset <= _maxOffsetPercent;

        public float GetOffsetDirection(float offset) => 
            Mathf.Sign(offset);
    }
    
    public struct OffsetCheckerResult
    {
        public bool isPermissible;
        public float offsetValue;
        public float percent;
        public float direction;
    }
}