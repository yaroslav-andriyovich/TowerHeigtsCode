using System;
using UnityEngine;

namespace CodeBase.Data.Level
{
    [Serializable]
    public class TowerStabilityData
    {
        [Tooltip("Max tower rotation angle")]
        [Range(0f, 360f)] public float maxAngle = 10f;
        [Tooltip("Affects the destabilization value of the tower")]
        [Min(0f)] public float destabilizationModifier = 2f;
        [Tooltip("Stabilizes the tower")]
        [Min(0f)] public float stabilizationByCombo = 0.25f;
        [Min(0f)] public int allowedBlocksNumber = 1;
    }
}