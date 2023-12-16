using UnityEngine;

namespace CodeBase.Extensions
{
    public static class GameObjectExtensions
    {
        public static void MakeActive(this GameObject go) =>
            go.SetActive(true);

        public static void MakeInactive(this GameObject go) =>
            go.SetActive(false);
    }
}