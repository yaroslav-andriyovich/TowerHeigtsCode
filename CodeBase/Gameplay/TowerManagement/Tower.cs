using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Extensions;
using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.BlockTracking;
using CodeBase.Gameplay.Combo;
using CodeBase.Gameplay.Stability;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.TowerManagement
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private TowerRotator _rotator;
        [SerializeField] private TowerFoundationProvider _foundation;
        [Space]
        [SerializeField] private AudioSource _collapseAudio;
        [SerializeField, Min(0)] private int _collapseMillisecondsInterval;

        public event Action OnCollapsed;
        public bool IsEmpty => _blocks.Count == 0;
        public int Count => _blocks.Count;
        
        private List<Block> _blocks;
        private BlockCombiner _blockCombiner;
        private OffsetChecker _offsetChecker;
        private TowerStability _towerStability;
        private ComboSystem _comboSystem;

        private void Awake() => 
            _blocks = new List<Block>();

        [Inject]
        public void Construct(
            BlockCombiner blockCombiner, 
            OffsetChecker offsetChecker,
            TowerStability towerStability,
            ComboSystem comboSystem
            )
        {
            _blockCombiner = blockCombiner;
            _offsetChecker = offsetChecker;
            _towerStability = towerStability;
            _comboSystem = comboSystem;
        }

        public void PutBlock(Block block, float offsetPercent)
        {
            ComboResult comboResult = _comboSystem.RegisterCombo(offsetPercent);
            
            block.transform.SetParent(transform);
            CombineBlock(block, comboResult.isCombo);
            _blocks.Add(block);
            RecalculateStability(offsetPercent, comboResult);
        }

        public Block TakeLastBlock()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Tower is empty!");

            Block block = GetLowestBlock();

            _blocks.Remove(block);
            block.transform.SetParent(null);

            return block;
        }

        public List<Transform> GetBlockTransforms() => 
            _blocks.Select(block => block.transform).ToList();

        public Block GetHighestBlock() => 
            IsEmpty 
                ? throw new InvalidOperationException("Tower is empty!")
                : _blocks[^1];

        public IObstacle GetFoundation() => 
            _foundation as IObstacle;

        public void Collapse()
        {
            _rotator.DisableComponent();
            _foundation.Hide();
            _collapseAudio.Play();
            CollapseBlocks(_blocks).Forget();
            OnCollapsed?.Invoke();
        }

        private void CombineBlock(Block block, bool withCombo)
        {
            if (IsEmpty)
                CombineWithFoundation(block);
            else
                CombineWithHighestBlock(block, withCombo);
        }

        private void CombineWithFoundation(Block block) => 
            _blockCombiner.Combine(block, GetFoundation());

        private void CombineWithHighestBlock(Block block, bool withCombo) => 
            _blockCombiner.Combine(block, GetHighestBlock(), withCombo);

        private void RecalculateStability(float offsetPercent, ComboResult comboResult)
        {
            if (Count <= 1)
                return;
            
            _towerStability.Recalculate(offsetPercent, comboResult);
            _rotator.RecalculateAngle(progressToMaxAngle: _towerStability.InvertedStabilityPercent);

            if (!_towerStability.IsStable)
                Collapse();
        }

        private Block GetLowestBlock() => 
            IsEmpty 
                ? throw new InvalidOperationException("Tower is empty!")
                : _blocks[0];

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