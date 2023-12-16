using System;
using UnityEngine;

namespace CodeBase.Data.Level
{
    [Serializable]
    public class TransformDescenderData
    {
        [Min(0f)] public float descendTime = 0.4f;
    }
}