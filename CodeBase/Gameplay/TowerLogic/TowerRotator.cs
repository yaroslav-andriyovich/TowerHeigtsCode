using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Gameplay.TowerLogic
{
    public class TowerRotator : MonoBehaviour
    {
        [SerializeField, Range(0f, 10f)] private float _maxAngle;
        [SerializeField, Range(-10f, 10f)] private float _angle;
        [SerializeField] private float _speed;

        private float _direction;

        private void Start() => 
            ChangeRandomDirection();

        private void Update()
        {
            IncrementAngle();
            ChangeDirection();
            ChangeRotation();
        }

        public void ChangeParams(float maxAngle, float speed)
        {
            _maxAngle = maxAngle;
            _speed = speed;
        }

        private void ChangeRandomDirection() => 
            _direction = Random.Range(0, 1) == 0 ? -1 : 1;

        private void IncrementAngle() => 
            _angle += _direction * _speed * Time.deltaTime;

        private void ChangeDirection()
        {
            if (_angle >= _maxAngle)
            {
                _angle = _maxAngle;
                _direction = -1;
            }
            else if (_angle <= -_maxAngle)
            {
                _angle = -_maxAngle;
                _direction = 1;
            }
        }

        private void ChangeRotation() => 
            transform.rotation = Quaternion.Euler(0, 0, SmoothAngleInOutSine());

        private float SmoothAngleInOutSine()
        {
            float t = Mathf.InverseLerp(-_maxAngle, _maxAngle, _angle);
            float smoothStepT = Mathf.SmoothStep(0, 1, t);
            float newAngle = Mathf.LerpAngle(-_maxAngle, _maxAngle, smoothStepT);
            
            return newAngle;
        }
    }
}