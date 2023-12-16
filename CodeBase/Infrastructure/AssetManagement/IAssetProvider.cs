using UnityEngine;

namespace CodeBase.Infrastructure.AssetManagement
{
    public interface IAssetProvider
    {
        GameObject Instantiate(GameObject prefab, Transform parent = null);
        T[] LoadAll<T>(string path) where T : Object;
    }
}