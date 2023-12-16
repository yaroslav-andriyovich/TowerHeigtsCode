using UnityEngine;

namespace CodeBase.Extensions
{
    public static class TransformExtensions
    {
        public static void MakeActive(this Transform transform) =>
            transform.gameObject.SetActive(true);

        public static void MakeInactive(this Transform transform) =>
            transform.gameObject.SetActive(false);
    }
}