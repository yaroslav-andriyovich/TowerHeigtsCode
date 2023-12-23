using DG.Tweening;
using UnityEngine;

namespace CodeBase.Gameplay.BaseBlock.Animator
{
    [CreateAssetMenu(fileName = "BlockAnimatorConfig", menuName = "Configs/Block/Animator", order = 1)]
    public class BlockAnimatorConfig : ScriptableObject
    {
        [Header("Crash")]
        public float crashPower = 4f;
        public float crashTime = 1.25f;
        public Vector2 crashEndPoint = new Vector2(2f, -6f);
        public float crashAngle = 270f;
        public Ease crashEase = Ease.Linear;
    }
}