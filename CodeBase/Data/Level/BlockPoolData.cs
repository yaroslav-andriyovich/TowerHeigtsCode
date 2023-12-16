using System;
using UnityEngine;

namespace CodeBase.Data.Level
{
    [Serializable]
    public class BlockPoolData
    {
        [Min(0)] public int capacity = 7;
        public GameObject blockPrefab;
    }
}