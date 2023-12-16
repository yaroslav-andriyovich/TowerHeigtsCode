using System;
using System.Collections.Generic;
using CodeBase.Data.Level;
using CodeBase.Extensions;
using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.BaseBlock.Factory;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using Unity.VisualScripting;
using UnityEngine;

namespace CodeBase.Gameplay.BlocksPool
{
    public class BlockPool : IBlockPool
    {
        public int Count => _queue.Count;
        public bool IsEmpty => _queue.Count == 0;

        private readonly IStaticDataService _staticDataService;
        private readonly IBlockFactory _blockFactory;
        
        private Queue<Block> _queue;
        private Transform _poolContainer;

        public BlockPool(IStaticDataService staticDataService, IBlockFactory blockFactory)
        {
            _staticDataService = staticDataService;
            _blockFactory = blockFactory;
        }

        public void CreatePool()
        {
            if (_queue != null && !IsEmpty)
                throw new InvalidOperationException("Pool already created!");
            
            BlockPoolData config = _staticDataService.ForCurrentMode().BlockPoolData;
            int capacity = config.capacity;
            GameObject blockPrefab = config.blockPrefab;
            
            _queue = new Queue<Block>(capacity);
            
            for (int i = 0; i < capacity; i++)
            {
                Block block = _blockFactory.Create(blockPrefab, parent: GetPoolContainer());
                
                PutBlock(block);
            }
        }

        public void PutBlock(Block block)
        {
            block.transform.parent = GetPoolContainer();
            block.transform.MakeInactive();
            
            _queue.Enqueue(block);
        }

        public Block TakeBlock()
        {
            if (IsEmpty)
                throw new IndexOutOfRangeException("Block pool is empty.");

            Block block = _queue.Dequeue();
            block.transform.MakeActive();

            return block;
        }

        private Transform GetPoolContainer()
        {
            if (_poolContainer == null)
                _poolContainer = CreatePoolContainer();

            return _poolContainer;
        }

        private Transform CreatePoolContainer()
        {
            GameObject gameObject = new GameObject("BlockPool");

            return gameObject.transform;
        }
    }
}