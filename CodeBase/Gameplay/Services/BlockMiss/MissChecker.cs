using System;
using CodeBase.Data.Level;
using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.Obstacle;
using CodeBase.Services.StaticData;
using CodeBase.Sounds;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Services.BlockMiss
{
    public class MissChecker : IInitializable, IDisposable, ITickable
    {
        public event Action OnMiss;
        public bool Enabled => _block != null && _obstacle != null;

        private readonly IStaticDataService _staticDataService;
        private readonly SoundPlayer _soundPlayer;

        private Transform _block;
        private Transform _obstacle;
        private float _deadZoneDistance;

        public MissChecker(IStaticDataService staticDataService, SoundPlayer soundPlayer)
        {
            _staticDataService = staticDataService;
            _soundPlayer = soundPlayer;
        }

        public void Initialize()
        {
            BlockMissCheckerData config = _staticDataService.ForCurrentMode().BlockMissCheckerData;
            _deadZoneDistance = config.deadZoneDistance;
        }

        public void Dispose() => 
            Stop();

        public void Tick()
        {
            if (!Enabled)
                return;
            
            CheckMiss();
        }

        public void Run(Block releasedBlock, IObstacle correctObstacle)
        {
            if (Enabled)
                throw new InvalidOperationException("Block miss checking has already begun!");
            
            _block = releasedBlock.transform;
            _obstacle = correctObstacle.transform;
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
            float waterline = obstaclePosition.y - _deadZoneDistance;
            float blockHeight = blockPosition.y;
            
            return waterline < blockHeight;
        }
    }
}