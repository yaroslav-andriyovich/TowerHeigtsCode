using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.BlocksPool;
using CodeBase.Gameplay.TowerManagement;
using CodeBase.Gameplay.TransformDescend;
using CodeBase.Sounds;

namespace CodeBase.Gameplay.BlockTracking
{
    public class BlockLanding
    {
        private readonly IBlockPool _blockPool;
        private readonly Tower _tower;
        private readonly TransformDescender _transformDescender;
        private readonly SoundPlayer _soundPlayer;

        public BlockLanding(
            IBlockPool blockPool, 
            Tower tower, 
            TransformDescender transformDescender,
            SoundPlayer soundPlayer
            )
        {
            _blockPool = blockPool;
            _tower = tower;
            _transformDescender = transformDescender;
            _soundPlayer = soundPlayer;
        }

        public void LandOnTower(Block block, CollisionOffset collisionOffset)
        {
            _transformDescender.CompleteMovement();
            AddBlockToTower(block, collisionOffset);
            TryDescendTowerBlocks();
            _transformDescender.Move(block.Height);
        }

        public void BounceToSide(Block block, float direction)
        {
            block.Bounce(direction);
            _soundPlayer.PlayBlockBounce();
        }

        private void AddBlockToTower(Block block, CollisionOffset collisionOffset)
        {
            block.Ground();
            _tower.PutBlock(block, collisionOffset.percent);
        }

        private void TryDescendTowerBlocks()
        {
            if (CanDescendOnlyBlocks())
                _transformDescender.DescendCustomTransforms(_tower.GetBlockTransforms());
        }

        private bool CanDescendOnlyBlocks() => 
            _blockPool.IsEmpty;
    }
}