using System;
using CodeBase.Gameplay.BaseBlock;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.BlockTracking
{
    public class CollisionDetector : ITickable, IDisposable
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
            Cleanup();

        public void Register(Block block)
        {
            if (_block != null)
                throw new InvalidOperationException("Already detecting!");
            
            _block = block;
        }

        public void Cleanup()
        {
            _block = null;
            Array.Clear(_hittableColliders, 0, _hittableColliders.Length);
        }

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
            obstacle = triggerCollider.GetComponent<IObstacle>();
            
            return obstacle != null;
        }
    }
}