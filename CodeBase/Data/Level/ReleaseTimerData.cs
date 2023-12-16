using System;
using UnityEngine.Rendering.PostProcessing;

namespace CodeBase.Data.Level
{
    [Serializable]
    public class ReleaseTimerData
    {
        [Min(0f)] public float duration = 4f;
    }
}