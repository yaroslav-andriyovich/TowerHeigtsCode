using System;
using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.Obstacle;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Services
{
    public class BlockCollisionDetector : ITickable, IDisposable
    {
        public event Action<Block, IObstacle> OnCollision;
        public bool Detecting => _block != null;
        
        private Collider[] _hittableColliders = new Collider[2];
        private Block _block;

        public void Tick()
        {
            if (Detecting && IsCollision(out IObstacle obstacle))
                OnCollision?.Invoke(_block, obstacle);
        }

        public void Dispose() => 
            StopDetect();

        public void StartDetect(Block block)
        {
            if (_block != null)
                throw new InvalidOperationException("Already detecting!");
            
            _block = block;
        }

        public void StopDetect() => 
            _block = null;

        private bool IsCollision(out IObstacle hit)
        {
            int hitCount = Physics.OverlapBoxNonAlloc(_block.transform.position, _block.transform.localScale / 2f, _hittableColliders);
            hit = null;

            for (int i = 0; i < hitCount; i++)
            {
                Collider otherCollider = _hittableColliders[i];
                
                if (IsNotOwnCollider(otherCollider) && IsObstacle(otherCollider, out IObstacle obstacle))
                    hit = obstacle;
            }

            return hit != null;
        }

        private bool IsNotOwnCollider(Collider otherCollider) => 
            otherCollider != _block.Collider;

        private bool IsObstacle(Collider triggerCollider, out IObstacle obstacle)
        {
            IObstacle obstacleComponent = triggerCollider.GetComponent<IObstacle>();
            obstacle = null;

            if (obstacleComponent != null)
                obstacle = obstacleComponent;
            
            return obstacle != null;
        }
    }
}