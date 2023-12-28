using System;
using System.Collections.Generic;
using CodeBase.Extensions;
using CodeBase.Gameplay.BaseBlock;
using CodeBase.Gameplay.BlockTracking;
using CodeBase.Gameplay.CameraManagement;
using CodeBase.Gameplay.TowerManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Stability
{
    public class TowerCollapse : MonoBehaviour
    {
        [SerializeField] private Tower _tower;
        [SerializeField] private TowerRotator _towerRotator;
        [SerializeField] private TowerFoundationProvider _foundation;
        [SerializeField] private AudioSource _collapseAudio;
        [SerializeField, Min(0)] private int _collapseMillisecondsInterval;
        
        public event Action OnCollapsed;
        
        private OffsetChecker _offsetChecker;
        private CameraShaker _cameraShaker;
        
        [Inject]
        public void Construct(OffsetChecker offsetChecker, CameraShaker cameraShaker)
        {
            _offsetChecker = offsetChecker;
            _cameraShaker = cameraShaker;
        }

        public void Collapse()
        {
            _towerRotator.DisableComponent();
            _foundation.Hide();
            _collapseAudio.Play();
            CollapseBlocks().Forget();
            _cameraShaker.Shake();
            OnCollapsed?.Invoke();
        }
        
        private async UniTaskVoid CollapseBlocks()
        {
            IReadOnlyList<Block> blocks = _tower.Blocks;

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