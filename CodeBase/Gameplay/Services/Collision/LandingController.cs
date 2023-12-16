using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.BlocksPool;
using CodeBase.Gameplay.Services.TransformDescend;
using CodeBase.Gameplay.TowerLogic;

namespace CodeBase.Gameplay.Services.Collision
{
    public class LandingController
    {
        private readonly IBlockPool _blockPool;
        private readonly Tower _tower;
        private readonly TransformDescender _transformDescender;

        public LandingController(
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
        {
            block.Ground();
            AddBlockToTower(block, withCombo);
        }

        public void LandWithCrash(Block block, float offsetDirection) => 
            block.Crash(offsetDirection);

        private void AddBlockToTower(Block block, bool withCombo)
        {
            _transformDescender.CompleteMovement();
            _tower.EnqueueBlock(block, withCombo);
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