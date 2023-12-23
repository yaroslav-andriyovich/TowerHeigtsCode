using System;
using CodeBase.Data.Level;
using CodeBase.Services.StaticData;
using CodeBase.Sounds;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Combo
{
    public class ComboSystem : IInitializable
    {
        public event Action<ComboResult> OnCombo;
        public int Streak
        {
            get => _streak;
            private set => _streak = Mathf.Min(value, _config.maxStreak);
        }
        public bool IsMaxStreak => Streak >= _config.maxStreak;

        private readonly IStaticDataService _staticDataService;
        private readonly SoundPlayer _soundPlayer;

        private ComboCheckerData _config;
        private int _streak;

        public ComboSystem(IStaticDataService staticDataService, SoundPlayer soundPlayer)
        {
            _staticDataService = staticDataService;
            _soundPlayer = soundPlayer;
        }

        public void Initialize() => 
            _config = _staticDataService.ForCurrentMode().ComboCheckerData;

        public ComboResult RegisterCombo(float offsetPercent)
        {
            bool isCombo = IsCombo(offsetPercent);
            
            if (isCombo)
            {
                AddStreak();
                _soundPlayer.PlayComboHit();
            }
            else
                Reset();

            ComboResult result = new ComboResult()
            {
                isCombo = isCombo,
                isMaxStreak = IsMaxStreak,
                streak = Streak
            };

            OnCombo?.Invoke(result);
            return result;
        }

        public void Reset() => 
            _streak = 0;

        private bool IsCombo(float offsetPercent) => 
            IsAllowableOffsetPercent(offsetPercent);

        private bool IsAllowableOffsetPercent(float percentOffset) => 
            percentOffset <= _config.maxOffsetPercent;

        private void AddStreak() => 
            Streak += 1;
    }

    public struct ComboResult
    {
        public bool isCombo;
        public bool isMaxStreak;
        public float streak;
    }
}