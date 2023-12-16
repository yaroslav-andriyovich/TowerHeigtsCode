using System;
using UnityEngine;

namespace CodeBase.Data.Level
{
    [Serializable]
    public class ComboCheckerData
    {
        [Range(0f, 1f)] public float maxComboOffsetPercent = 0.1f;
    }
}