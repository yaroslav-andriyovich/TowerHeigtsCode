using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.BlocksPool;
using CodeBase.Gameplay.TowerManagement;
using CodeBase.Gameplay.TransformDescend;

namespace CodeBase.Gameplay.BlockTracking
{
    public class BlockLanding
    {
        private readonly IBlockPool _blockPool;
        private readonly Tower _tower;
        private readonly TransformDescender _transformDescender;

        public BlockLanding(
            IBlockPool blockPool, 
            Tower tower, 
            TransformDescender transformDescender
            )
        {
            _blockPool = blockPool;
            _tower = tower;
            _transformDescender = transformDescender;
        }

        public void Land(Block block, bool withCombo = false)
        //public void Land(Block block, OffsetCheckerResult offset)
        {
            block.Ground();
            AddBlockToTower(block, withCombo);
            //AddBlockToTower(block, offset);
        }

        public void LandWithCrash(Block block, float offsetDirection) => 
            block.Crash(offsetDirection);

        private void AddBlockToTower(Block block, bool withCombo)
        //private void AddBlockToTower(Block block, OffsetCheckerResult offset)
        {
            _transformDescender.CompleteMovement();
            _tower.EnqueueBlock(block, withCombo);
            //_tower.EnqueueBlock(block, offset);
            DescendTowerBlocks();
            _transformDescender.Move(block.Height);
        }

        private void DescendTowerBlocks()
        {
            if (CanDescendOnlyBlocks())
                _transformDescender.DescendCustomTransforms(_tower.GetBlockTransforms());
        }

        private bool CanDescendOnlyBlocks() => 
            _blockPool.IsEmpty;
    }
}