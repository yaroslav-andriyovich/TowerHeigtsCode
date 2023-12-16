using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        private readonly IInstantiator _instantiator;

        public AssetProvider(IInstantiator instantiator) => 
            _instantiator = instantiator;

        public GameObject Instantiate(GameObject prefab, Transform parent = null) => 
            _instantiator.InstantiatePrefab(prefab, parent);

        public T[] LoadAll<T>(string path) where T : Object => 
            Resources.LoadAll<T>(path);
    }
}