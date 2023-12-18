using System.Collections;
using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.BlocksPool;
using CodeBase.Gameplay.BlockTracking;
using CodeBase.Gameplay.RopeManagement;
using CodeBase.Gameplay.TowerManagement;
using CodeBase.Gameplay.TransformDescend;
using UnityEngine;
using Zenject;

namespace CodeBase.Extensions
{
    public class AutoStackerForTest : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float _interval;
        [SerializeField] private bool _printCount;
        
        private Rope _rope;
        private Block _releasedBlock;
        private RopeAttachment _ropeAttachment;
        private CollisionValidator _collisionValidator;
        private MissChecker _missChecker;

        [Inject]
        public void Construct(
            Rope rope,
            IBlockPool blockPool,
            Tower tower, 
            TransformDescender transformDescender,
            RopeAttachment ropeAttachment,
            CollisionValidator collisionValidator,
            MissChecker missChecker
        )
        {
            _rope = rope;
            //_towerBlockAdditionController = new TowerBlockAdditionController(blockPool, tower, transformDescender);
            _ropeAttachment = ropeAttachment;
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
                
                //_releasedBlock = _rope.ReleaseBlock();
                _releasedBlock.Ground();
                //_collisionValidator.Cleanup();
                _missChecker.Stop();
                //_towerBlockAdditionController.AddBlock(_releasedBlock, true);
                _releasedBlock = null;
                _ropeAttachment.AttachBlock();
                
                ++count;
                if (_printCount)
                    Debug.Log(++count);
            }
        }

        private IEnumerator WaitBlockBind()
        {
            while (!_rope.HasBlock)
            {
                yield return null;
            }
        }
    }
}