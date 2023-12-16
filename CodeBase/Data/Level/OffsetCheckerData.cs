using System;
using UnityEngine;

namespace CodeBase.Data.Level
{
    [Serializable]
    public class OffsetCheckerData
    {
        [Range(0f, 1f)] public float maxOffsetPercent = 0.45f;
    }
}