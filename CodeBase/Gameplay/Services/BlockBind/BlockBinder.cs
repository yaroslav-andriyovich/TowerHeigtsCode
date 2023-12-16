using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.BlocksPool;
using CodeBase.Gameplay.HoistingRopeLogic;
using CodeBase.Gameplay.TowerLogic;

namespace CodeBase.Gameplay.Services.BlockBind
{
    public class BlockBinder
    {
        public bool IsCanUnbind => _hoistingRope.HasBlock; 
        
        private readonly IBlockPool _blockPool;
        private readonly Tower _tower;
        private readonly HoistingRope _hoistingRope;

        public BlockBinder(
            IBlockPool blockPool, 
            Tower tower, 
            HoistingRope hoistingRope
            )
        {
            _blockPool = blockPool;
            _tower = tower;
            _hoistingRope = hoistingRope;
        }

        public Block BindNext()
        {
            if (_hoistingRope.HasBlock)
                return null;
            
            Block nextBlock = GetBlock();
            nextBlock.Restore();
            
            _hoistingRope.BindBlock(nextBlock);
            return nextBlock;
        }

        public Block Unbind() => 
            !IsCanUnbind 
                ? null 
                : _hoistingRope.ReleaseBlock();

        private Block GetBlock() => 
            _blockPool.IsEmpty 
                ? _tower.DequeueBlock() 
                : _blockPool.TakeBlock();
    }
}