using System.Collections.Generic;
using System.Linq;
using CodeBase.Extensions;
using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.Obstacle;
using CodeBase.Gameplay.Services.Collision;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.TowerLogic
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private TowerRotator _rotator;
        [SerializeField] private TowerFoundationProvider _foundation;
        [Space]
        [SerializeField] private AudioSource _collapseAudio;
        [SerializeField, Min(0)] private int _collapseMillisecondsInterval;

        public bool IsEmpty => _blocks.Count == 0;
        public int Count => _blocks.Count;
        
        private List<Block> _blocks;
        private BlockCombiner _blockCombiner;
        private OffsetChecker _offsetChecker;

        private void Awake()
        {
            _blocks = new List<Block>();
            _blockCombiner = new BlockCombiner();
        }

        [Inject]
        public void Construct(OffsetChecker offsetChecker) => 
            _offsetChecker = offsetChecker;

        public void EnqueueBlock(Block block, bool isCombo = false)
        {
            SetBlockParent(block, transform);
            CombineBlock(block, isCombo);
            _blocks.Add(block);
        }

        public Block DequeueBlock()
        {
            if (IsEmpty)
                return null;

            Block block = GetLowestBlock();
            
            _blocks.Remove(block);
            SetBlockParent(block, null);

            return block;
        }

        public List<Transform> GetBlockTransforms() => 
            _blocks.Select(block => block.transform).ToList();

        public Block GetHighestBlock() => 
            IsEmpty 
                ? null
                : _blocks[^1];

        public IObstacle GetFoundation() => 
            _foundation as IObstacle;

        public void ChangeRotationParams(float maxAngle, float speed) => 
            _rotator.ChangeParams(maxAngle, speed);

        public void Collapse()
        {
            _rotator.DisableComponent();
            HideFoundation();
            _collapseAudio.Play();
            CollapseBlocks(_blocks).Forget();
        }
        
        private void SetBlockParent(Block block, Transform parent) => 
            block.transform.parent = parent;

        private void CombineBlock(Block block, bool isCombo)
        {
            if (IsEmpty)
                CombineWithFoundation(block);
            else
                CombineWithHighestBlock(block, isCombo);
        }

        private void CombineWithFoundation(Block block) => 
            _blockCombiner.Combine(block, GetFoundation());

        private void CombineWithHighestBlock(Block block, bool isCombo) => 
            _blockCombiner.Combine(block, GetHighestBlock(), isCombo);
        
        private Block GetLowestBlock() => 
            IsEmpty 
                ? null
                : _blocks[0];
        
        private void HideFoundation() => 
            _foundation.gameObject.SetActive(false);

        private async UniTaskVoid CollapseBlocks(IReadOnlyList<Block> blocks)
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                float collapseDirection;
                Vector3 blockPosition = blocks[i].transform.position;

                if (i == 0)
                    collapseDirection = GetCollapseDirection(_foundation, blockPosition);
                else
                {
                    IObstacle previousBlock = blocks[i - 1];
                    collapseDirection = GetCollapseDirection(previousBlock, blockPosition);
                }

                blocks[i].Collapse(collapseDirection);
                
                await UniTask.Delay(_collapseMillisecondsInterval);
            }
        }

        private float GetCollapseDirection(IObstacle obstacle, Vector3 blockPosition)
        {
            float offset = _offsetChecker.GetObstacleOffset(obstacle, blockPosition);
            float collapseDirection = _offsetChecker.GetOffsetDirection(offset);
            
            return collapseDirection;
        }
    }
}