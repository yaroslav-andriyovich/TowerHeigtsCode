using UnityEngine;

namespace CodeBase.Gameplay.BaseBlock
{
    public class BlockOffsetTracker : MonoBehaviour
    {
        public float HorizontalOffset { get; private set; }
        
        private float _lastPositionX;

        private void Update()
        {
            CalculateHorizontalOffset();
            SaveLastPositionX();
        }

        private void OnEnable() => 
            Reset();

        private void CalculateHorizontalOffset() => 
            HorizontalOffset = transform.position.x - _lastPositionX;

        private void SaveLastPositionX() => 
            _lastPositionX = transform.position.x;

        private void Reset()
        {
            HorizontalOffset = 0f;
            _lastPositionX = 0f;
        }
    }
}