using System;
using UnityEngine.Rendering.PostProcessing;

namespace CodeBase.Data.Level
{
    [Serializable]
    public class BlockMissCheckerData
    {
        [Min(0f)] public float deadZoneDistance = 3f;
    }
}