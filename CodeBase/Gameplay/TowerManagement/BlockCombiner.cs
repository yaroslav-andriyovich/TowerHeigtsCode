using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.BlockTracking;
using UnityEngine;

namespace CodeBase.Gameplay.TowerManagement
{
    public class BlockCombiner
    {
        public void Combine(Block block, IObstacle obstacle, bool withCombo = false) => 
            AnchorOverObstacle(block, obstacle, withCombo);

        private void AnchorOverObstacle(Block block, IObstacle obstacle, bool withCombo = false)
        {
            Vector3 blockPosition = obstacle.transform.localPosition;
            
            float blockOffset = block.Height / 2f;
            float obstacleOffset = obstacle.Height / 2f;
            float anchorPointOffset = blockOffset + obstacleOffset;

            if (!withCombo)
                blockPosition.x = block.transform.localPosition.x;
            
            blockPosition.y += anchorPointOffset;

            block.transform.localPosition = blockPosition;
            block.transform.localRotation = obstacle.transform.localRotation;
        }
    }
}