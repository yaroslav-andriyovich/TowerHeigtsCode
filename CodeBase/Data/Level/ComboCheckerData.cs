using System;
using UnityEngine;

namespace CodeBase.Data.Level
{
    [Serializable]
    public class ComboCheckerData
    {
        [Range(0f, 1f)] public float maxOffsetPercent = 0.1f;
        [Min(0)] public int maxStreak = 5;
    }
}