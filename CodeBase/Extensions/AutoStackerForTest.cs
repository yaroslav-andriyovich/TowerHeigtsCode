using System.Collections;
using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.BlockTracking;
using CodeBase.Gameplay.RopeManagement;
using CodeBase.Gameplay.Stability;
using CodeBase.Gameplay.TowerManagement;
using UnityEngine;
using Zenject;

namespace CodeBase.Extensions
{
    public class AutoStackerForTest : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float _interval;
        [SerializeField] private bool _printCount;

        private CollisionDetector _collisionDetector;
        private BlockTracker _blockTracker;
        private Rope _rope;
        private CollisionHandler _collisionHandler;
        private TowerCollapse _tower;

        private Block _block;

        [Inject]
        public void Construct(
            CollisionHandler collisionHandler,
            BlockTracker blockTracker,
            Rope rope,
            CollisionDetector collisionDetector,
            TowerCollapse tower
        )
        {
            _collisionDetector = collisionDetector;
            _blockTracker = blockTracker;
            _rope = rope;
            _collisionHandler = collisionHandler;
            _tower = tower;
        }

        private void Start()
        {
            _collisionDetector.Dispose();
            _blockTracker.Dispose();
            _rope.Movement.DisableComponent();
            _rope.OnAttached += OnBlockAttached;
            _tower.OnCollapsed += () => gameObject.MakeInactive();

            StartCoroutine(Procedure());
        }

        private void OnDestroy() => 
            _rope.OnAttached -= OnBlockAttached;

        private void OnBlockAttached(Block block) => 
            _block = block;

        private IEnumerator Procedure()
        {
            int count = 0;

            yield return WaitRopeAttachment();

            while (enabled)
            {
                yield return new WaitForSeconds(_interval);
                yield return WaitRopeAttachment();

                CollisionOffset collisionOffset = new CollisionOffset()
                {
                    isAllowable = true,
                    offsetValue = 0f,
                    percent = Random.Range(0f, 0.13f)
                };
                
                _rope.ReleaseBlock();
                //_collisionHandler.LandBlockOnTower(_block, collisionOffset);

                ++count;
                if (_printCount)
                    Debug.Log(++count);
            }
            
            _block = null;
        }

        private IEnumerator WaitRopeAttachment()
        {
            while (_block == null)
            {
                yield return null;
            }
        }
    }
}