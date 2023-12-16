using DG.Tweening;
using UnityEngine;

namespace CodeBase.Gameplay.BaseBlock.Animator
{
    [CreateAssetMenu(fileName = "BlockAnimatorConfig", menuName = "Configs/Block/Animator", order = 1)]
    public class BlockAnimatorConfig : ScriptableObject
    {
        [Header("Crash")]
        [SerializeField] private float _crashPower;
        [SerializeField] private float _crashTime;
        [SerializeField] private Vector2 _crashEndPoint;
        [SerializeField] private float _crashAngle;
        [SerializeField] private Ease _crashEase;

        public float crashPower => _crashPower;
        public float crashTime => _crashTime;
        public Vector2 crashEndPoint => _crashEndPoint;
        public float crashAngle => _crashAngle;
        public Ease crashEase => _crashEase;
    }
}