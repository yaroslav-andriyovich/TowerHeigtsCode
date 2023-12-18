using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.BlocksPool;
using CodeBase.Gameplay.TowerManagement;

namespace CodeBase.Gameplay.RopeManagement
{
    public class RopeAttachment
    {
        private readonly Rope _rope;
        private readonly IBlockPool _blockPool;
        private readonly Tower _tower;
        private readonly ReleaseTimer _releaseTimer;

        public RopeAttachment(
            Rope rope, 
            IBlockPool blockPool, 
            Tower tower,
            ReleaseTimer releaseTimer
            )
        {
            _rope = rope;
            _blockPool = blockPool;
            _tower = tower;
            _releaseTimer = releaseTimer;
        }

        public void AttachBlock()
        {
            if (_rope.HasBlock)
                return;
            
            Block newBlock = GetBlock();
            newBlock.Restore();
            
            _rope.AttachBlock(newBlock);
            _releaseTimer.Start();
        }

        private Block GetBlock() => 
            _blockPool.IsEmpty 
                ? _tower.DequeueBlock() 
                : _blockPool.TakeBlock();
    }
}