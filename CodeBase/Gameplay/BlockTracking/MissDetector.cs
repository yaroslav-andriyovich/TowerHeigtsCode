using System;
using CodeBase.Data.Level;
using CodeBase.Gameplay.BaseBlock;
using CodeBase.Services.StaticData;
using CodeBase.Sounds;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.BlockTracking
{
    public class MissDetector : IInitializable, IDisposable, ITickable
    {
        public event Action OnMiss;
        public bool Enabled => _block != null && _obstacle != null;

        private readonly IStaticDataService _staticDataService;
        private readonly ObstacleValidator _obstacleValidator;
        private readonly SoundPlayer _soundPlayer;

        private Transform _block;
        private Transform _obstacle;
        private MissDetectorData _config;

        public MissDetector(
            IStaticDataService staticDataService,
            ObstacleValidator obstacleValidator,
            SoundPlayer soundPlayer
            )
        {
            _staticDataService = staticDataService;
            _obstacleValidator = obstacleValidator;
            _soundPlayer = soundPlayer;
        }

        public void Initialize() => 
            _config = _staticDataService.ForCurrentMode()._missDetectorData;

        public void Dispose() => 
            Stop();

        public void Tick()
        {
            if (!Enabled)
                return;
            
            CheckMiss();
        }

        public void Run(Block releasedBlock)
        {
            if (Enabled)
                throw new InvalidOperationException("Block miss checking has already begun!");
            
            _block = releasedBlock.transform;
            _obstacle = _obstacleValidator.GetCorrect().transform;
        }

        public void Stop()
        {
            _block = null;
            _obstacle = null;
        }

        private void CheckMiss()
        {
            if (BlockAboveObstacle(_block.position, _obstacle.position))
                return;
            
            NotifyMissed();
            Stop();
        }

        private void NotifyMissed()
        {
            _soundPlayer.PlayMiss();
            OnMiss?.Invoke();
        }

        private bool BlockAboveObstacle(Vector3 blockPosition, Vector3 obstaclePosition)
        {
            float waterline = obstaclePosition.y - _config.deadZoneDistance;
            float blockHeight = blockPosition.y;
            
            return waterline < blockHeight;
        }
    }
}