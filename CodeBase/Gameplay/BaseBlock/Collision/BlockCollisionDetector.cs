using System;
using CodeBase.Gameplay.Obstacle;
using UnityEngine;

namespace CodeBase.Gameplay.BaseBlock.Collision
{
    public class BlockCollisionDetector : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        
        public event Action<IObstacle> OnCollision;

        private Collider[] _hittableColliders = new Collider[2];

        private void Update()
        {
            if (IsCollision(out IObstacle obstacle))
                OnCollision?.Invoke(obstacle);
        }

        private bool IsCollision(out IObstacle hit)
        {
            int hitCount = Physics.OverlapBoxNonAlloc(transform.position, transform.localScale / 2f, _hittableColliders);
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
            otherCollider != _collider;
        
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