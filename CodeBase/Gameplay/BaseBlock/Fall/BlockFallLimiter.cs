using UnityEngine;

namespace CodeBase.Gameplay.BaseBlock.Fall
{
    public class BlockFallLimiter : MonoBehaviour
    {
        [SerializeField] private float _redLineY;

        private void Update()
        {
            if (transform.position.y <= _redLineY)
                gameObject.SetActive(false);
        }
    }
}