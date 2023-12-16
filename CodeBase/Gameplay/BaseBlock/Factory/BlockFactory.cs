using CodeBase.Infrastructure.AssetManagement;
using UnityEngine;

namespace CodeBase.Gameplay.BaseBlock.Factory
{
    public class BlockFactory : IBlockFactory
    {
        private readonly IAssetProvider _assetProvider;

        public BlockFactory(IAssetProvider assetProvider) => 
            _assetProvider = assetProvider;

        public Block Create(GameObject prefab, Transform parent = null)
        {
            if (prefab.GetComponent<Block>() == null)
                throw new MissingComponentException($"Prefab {prefab.name} does not have the required IBlock component.");

            GameObject blockInstance = _assetProvider.Instantiate(prefab, parent);
            Block block = blockInstance.GetComponent<Block>();
            
            return block;
        }
    }
}