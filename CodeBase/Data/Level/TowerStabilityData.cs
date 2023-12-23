using System;
using UnityEngine;

namespace CodeBase.Data.Level
{
    [Serializable]
    public class TowerStabilityData
    {
        [Range(0f, 360f)] public float maxAngle = 10f;
        [Range(0f, 1f)] public float minDeviationPercent = 0.3f;
        public AnimationCurve stabilityCurve;
        [Min(0f)] public float reduceMultiplier = 2f;
        [Min(1f)] public float improveMultiplier = 1.2f;
    }
}