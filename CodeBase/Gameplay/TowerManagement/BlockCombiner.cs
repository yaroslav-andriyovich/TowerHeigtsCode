using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.BlockTracking;
using UnityEngine;

namespace CodeBase.Gameplay.TowerManagement
{
    public class BlockCombiner
    {
        public void Combine(Block block, IObstacle obstacle, bool isCombo = false) => 
            AnchorOverObstacle(block, obstacle, isCombo);

        private void AnchorOverObstacle(Block block, IObstacle obstacle, bool isCombo = false)
        {
            Vector3 blockPosition = obstacle.transform.localPosition;
            
            float blockOffset = block.Height / 2f;
            float obstacleOffset = obstacle.Height / 2f;
            float anchorPointOffset = blockOffset + obstacleOffset;

            if (!isCombo)
                blockPosition.x = block.transform.localPosition.x;
            
            blockPosition.y += anchorPointOffset;

            block.transform.localPosition = blockPosition;
            block.transform.localRotation = obstacle.transform.localRotation;
        }
    }
}