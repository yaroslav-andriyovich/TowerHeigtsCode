using CodeBase.Gameplay.BaseBlock;
using UnityEngine;

namespace CodeBase.Gameplay.BlocksPool
{
    public interface IBlockPool
    {
        int Count { get; }
        bool IsEmpty { get; }
        
        void CreatePool();
        void PutBlock(Block block);
        Block TakeBlock();
    }
}