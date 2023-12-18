using System;
using CodeBase.Data.Level;
using CodeBase.Gameplay.TowerManagement;
using CodeBase.Services.StaticData;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Stability
{
    public class TowerStability : IInitializable
    {
        public event Action OnCollapsed;
        public bool IsStable => _weight < _config.maxAngle;

        private readonly Tower _tower;
        private readonly IStaticDataService _staticDataService;
        private readonly StabilityView _stabilityView;
        private readonly CameraShaker _cameraShaker;

        private float MaxAngle => _config.maxAngle;
        private float DestabilizationModifier => _config.destabilizationModifier;
        private float StabilizationByCombo => _config.stabilizationByCombo;
        private bool IsInsufficientBlocksNumber => _tower.Count <= _config.allowedBlocksNumber;

        private TowerStabilityData _config;
        private float _weight;

        public TowerStability(
            IStaticDataService staticDataService, 
            Tower tower, 
            StabilityView stabilityView,
            CameraShaker cameraShaker
        )
        {
            _staticDataService = staticDataService;
            _tower = tower;
            _stabilityView = stabilityView;
            _cameraShaker = cameraShaker;
        }

        public void Initialize() => 
            _config = _staticDataService.ForCurrentMode().TowerStabilityData;

        public void Change(float offsetPercent, bool isCombo)
        {
            if (IsInsufficientBlocksNumber)
                return;

            RecalculateStability(offsetPercent, isCombo);
            Apply();
            TryCollapseTower();
        }

        private void RecalculateStability(float offsetPercent, bool isCombo)
        {
            if (isCombo)
                AddStabilityByCombo();
            else if (IsNotMaxDestabilization())
                ReduceStability(offsetPercent);
        }

        private void AddStabilityByCombo()
        {
            float stabilizedWeight = _weight - StabilizationByCombo;
            SetWeight(stabilizedWeight);
        }

        private void SetWeight(float weight) => 
            _weight = Mathf.Clamp(weight, min: 0f, MaxAngle);

        private bool IsNotMaxDestabilization() => 
            _weight < MaxAngle;

        private void ReduceStability(float reductionValue)
        {
            float destabilizedWeight = _weight + reductionValue * DestabilizationModifier;
            SetWeight(destabilizedWeight);
        }

        private void Apply()
        {
            _tower.ChangeRotationParams(maxAngle: _weight, speed: _weight / 2f);
            _stabilityView.SetProgress(current: _weight, max: MaxAngle);
        }

        private void TryCollapseTower()
        {
            if (IsStable)
                return;
            
            _tower.Collapse();
            _cameraShaker.Shake();
            OnCollapsed?.Invoke();
        }
    }
}