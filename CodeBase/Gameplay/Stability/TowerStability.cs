using CodeBase.Data.Level;
using CodeBase.Gameplay.Combo;
using CodeBase.Gameplay.TowerManagement;
using CodeBase.Services.StaticData;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Stability
{
    public class TowerStability : MonoBehaviour
    {
        [SerializeField] private Tower _tower;
        [SerializeField] private TowerRotator _towerRotator;
        [SerializeField] private TowerCollapse _towerCollapse;
        
        public bool IsStable => StabilityPercent > MinStability;
        public float MinStability => StabilityCurve.keys[0].time;
        public float MaxStability => StabilityCurve.keys[^1].time;
        public float StabilityPercent
        {
            get => _stabilityPercent;
            private set => _stabilityPercent = StabilityCurve.Evaluate(value);
        }
        public float InvertedStabilityPercent => 1f - StabilityPercent;

        private IStaticDataService _staticDataService;
        private StabilityView _stabilityView;
        private TowerStabilityData _config;
        private float _stabilityPercent;

        private AnimationCurve StabilityCurve => _config.stabilityCurve;

        private void Start()
        {
            _tower.OnBlockAdded += OnOnBlockAddedToTower;
            
            _config = _staticDataService.ForCurrentMode().TowerStabilityData;
            Restore();
        }

        private void OnDestroy() => 
            _tower.OnBlockAdded -= OnOnBlockAddedToTower;

        [Inject]
        public void Construct(IStaticDataService staticDataService, StabilityView stabilityView)
        {
            _staticDataService = staticDataService;
            _stabilityView = stabilityView;
        }

        private void OnOnBlockAddedToTower(TowerBlockAddedResult result) => 
            Recalculate(result.offsetPercent, result.comboResult);

        private void Restore() => 
            _stabilityPercent = MaxStability;

        private void Recalculate(float offsetPercent, ComboResult comboResult)
        {
            if (_tower.Count <= 1)
                return;
            
            if (!comboResult.isCombo)
                ReduceStability(reduceValue01: offsetPercent);
            else if (comboResult.isMaxStreak)
                ImproveStability();
            
            _stabilityView.SetProgress(InvertedStabilityPercent, MaxStability);
            _towerRotator.RecalculateAngle(progressToMaxAngle: InvertedStabilityPercent);
            
            if (!IsStable)
                _towerCollapse.Collapse();
        }

        private void ImproveStability() => 
            StabilityPercent *= _config.improveMultiplier;

        private void ReduceStability(float reduceValue01)
        {
            reduceValue01 = Mathf.Clamp01(reduceValue01);
            
            StabilityPercent -= reduceValue01 * _config.reduceMultiplier;
        }
    }
}