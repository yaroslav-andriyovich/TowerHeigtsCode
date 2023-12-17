using CodeBase.Gameplay.Obstacle;
using UnityEngine;

namespace CodeBase.Gameplay.BaseBlock
{
    public abstract class Block : MonoBehaviour, IObstacle
    {
        [SerializeField] protected BoxCollider _collider;
        
        public virtual float Height => _collider.size.y * transform.localScale.y;
        public virtual float Width => _collider.size.x * transform.localScale.x;
        public BoxCollider Collider => _collider;

        public abstract void Restore();
        public abstract void EnableFalling();
        public abstract void DisableFalling();
        public abstract void Ground();
        public abstract void Crash(float offsetDirection);
        public abstract void Collapse(float collapseDirection);
    }
}