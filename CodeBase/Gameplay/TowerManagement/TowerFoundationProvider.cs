using CodeBase.Extensions;
using CodeBase.Gameplay.BlockTracking;
using UnityEngine;

namespace CodeBase.Gameplay.TowerManagement
{
    [RequireComponent(typeof(BoxCollider))]
    public class TowerFoundationProvider : MonoBehaviour, IObstacle
    {
        public float Height => _collider.size.y * transform.localScale.y;
        public float Width => _collider.size.x * transform.localScale.x;

        private BoxCollider _collider;

        private void Awake() => 
            _collider = GetComponent<BoxCollider>();

        public void Hide() => 
            gameObject.MakeInactive();
    }
}