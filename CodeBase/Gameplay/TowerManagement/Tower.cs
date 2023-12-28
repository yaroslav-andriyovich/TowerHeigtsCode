using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.BlockTracking;
using CodeBase.Gameplay.Combo;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.TowerManagement
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private TowerFoundationProvider _foundation;

        public event Action<TowerBlockAddedResult> OnBlockAdded;
        public bool IsEmpty => _blocks.Count == 0;
        public int Count => _blocks.Count;
        public IReadOnlyList<Block> Blocks => _blocks;

        private List<Block> _blocks;
        private BlockCombiner _blockCombiner;
        private ComboSystem _comboSystem;

        private void Awake() => 
            _blocks = new List<Block>();

        [Inject]
        public void Construct(BlockCombiner blockCombiner, ComboSystem comboSystem)
        {
            _blockCombiner = blockCombiner;
            _comboSystem = comboSystem;
        }

        public void PutBlock(Block block, float offsetPercent)
        {
            ComboResult comboResult = _comboSystem.RegisterCombo(offsetPercent);
            
            block.transform.SetParent(transform);
            CombineBlock(block, comboResult.isCombo);
            _blocks.Add(block);

            TowerBlockAddedResult result = new TowerBlockAddedResult()
            {
                offsetPercent = offsetPercent,
                comboResult = comboResult,
            };

            OnBlockAdded?.Invoke(result);
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

        private Block GetLowestBlock() => 
            IsEmpty 
                ? throw new InvalidOperationException("Tower is empty!")
                : _blocks[0];
    }

    public struct TowerBlockAddedResult
    {
        public float offsetPercent;
        public ComboResult comboResult;
    }
}