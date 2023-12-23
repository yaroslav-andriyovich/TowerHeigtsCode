using CodeBase.Data.Level;
using CodeBase.Gameplay.Combo;
using CodeBase.Services.StaticData;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Stability
{
    public class TowerStability : IInitializable
    {
        public bool IsStable => StabilityPercent > MinStability;
        private float MinStability => StabilityCurve.keys[0].time;
        public float MaxStability => StabilityCurve.keys[^1].time;
        public float StabilityPercent
        {
            get => _stabilityPercent;
            private set => _stabilityPercent = StabilityCurve.Evaluate(value);
        }
        public float InvertedStabilityPercent => 1f - StabilityPercent;

        private readonly IStaticDataService _staticDataService;
        private readonly StabilityView _stabilityView;
        private TowerStabilityData _config;
        private float _stabilityPercent;

        private AnimationCurve StabilityCurve => _config.stabilityCurve;


        public TowerStability(IStaticDataService staticDataService, StabilityView stabilityView)
        {
            _staticDataService = staticDataService;
            _stabilityView = stabilityView;
        }

        public void Initialize()
        {
            _config = _staticDataService.ForCurrentMode().TowerStabilityData;
            Restore();
        }
        
        public void Recalculate(float offsetPercent, ComboResult comboResult)
        {
            if (!comboResult.isCombo)
                ReduceStability(reduceValue01: offsetPercent);
            else if (comboResult.isMaxStreak)
                ImproveStability();
            
            _stabilityView.SetProgress(InvertedStabilityPercent, MaxStability);
        }

        public void ImproveStability() => 
            StabilityPercent *= _config.improveMultiplier;

        public void ReduceStability(float reduceValue01)
        {
            reduceValue01 = Mathf.Clamp01(reduceValue01);
            
            StabilityPercent -= reduceValue01 * _config.reduceMultiplier;
        }

        public void Restore() => 
            _stabilityPercent = MaxStability;
    }
}