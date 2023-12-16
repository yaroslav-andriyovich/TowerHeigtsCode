using UnityEngine;

namespace CodeBase.Gameplay.BaseBlock.Factory
{
    public interface IBlockFactory
    {
        Block Create(GameObject prefab, Transform parent);
    }
}