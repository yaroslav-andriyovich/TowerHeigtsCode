using System.Collections;
using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.BlocksPool;
using CodeBase.Gameplay.HoistingRopeLogic;
using CodeBase.Gameplay.Services;
using CodeBase.Gameplay.Services.BlockBind;
using CodeBase.Gameplay.Services.BlockMiss;
using CodeBase.Gameplay.Services.Collision;
using CodeBase.Gameplay.Services.TransformDescend;
using CodeBase.Gameplay.TowerLogic;
using UnityEngine;
using Zenject;

namespace CodeBase.Extensions
{
    public class AutoStackerForTest : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float _interval;
        [SerializeField] private bool _printCount;
        
        private HoistingRope _hoistingRope;
        private Block _releasedBlock;
        private BlockBinder _blockBinder;
        private CollisionValidator _collisionValidator;
        private MissChecker _missChecker;

        [Inject]
        public void Construct(
            HoistingRope hoistingRope,
            IBlockPool blockPool,
            Tower tower, 
            TransformDescender transformDescender,
            BlockBinder blockBinder,
            CollisionValidator collisionValidator,
            MissChecker missChecker
        )
        {
            _hoistingRope = hoistingRope;
            //_towerBlockAdditionController = new TowerBlockAdditionController(blockPool, tower, transformDescender);
            _blockBinder = blockBinder;
            _collisionValidator = collisionValidator;
            _missChecker = missChecker;
        }

        private IEnumerator Start()
        {
            int count = 0;

            yield return WaitBlockBind();

            while (enabled)
            {
                yield return WaitBlockBind();
                yield return new WaitForSeconds(_interval);
                
                _releasedBlock = _hoistingRope.ReleaseBlock();
                _releasedBlock.Ground();
                //_collisionValidator.Cleanup();
                _missChecker.Stop();
                //_towerBlockAdditionController.AddBlock(_releasedBlock, true);
                _releasedBlock = null;
                _blockBinder.BindNext();
                
                ++count;
                if (_printCount)
                    Debug.Log(++count);
            }
        }

        private IEnumerator WaitBlockBind()
        {
            while (!_hoistingRope.HasBlock)
            {
                yield return null;
            }
        }
    }
}