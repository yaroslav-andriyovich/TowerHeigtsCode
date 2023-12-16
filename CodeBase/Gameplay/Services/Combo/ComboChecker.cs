using CodeBase.Data.Level;
using CodeBase.Gameplay.TowerLogic;
using CodeBase.Services.StaticData;
using CodeBase.Sounds;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Services.Combo
{
    public class ComboChecker : IInitializable
    {
        public int Count { get; private set; }
        public float Streak { get; private set; }

        private readonly IStaticDataService _staticDataService;
        private readonly Tower _tower;
        private readonly SoundPlayer _soundPlayer;

        private float _maxComboOffsetPercent;

        public ComboChecker(IStaticDataService staticDataService, Tower tower, SoundPlayer soundPlayer)
        {
            _staticDataService = staticDataService;
            _tower = tower;
            _soundPlayer = soundPlayer;
        }

        public void Initialize()
        {
            ComboCheckerData config = _staticDataService.ForCurrentMode().ComboCheckerData;
            _maxComboOffsetPercent = config.maxComboOffsetPercent;
        }

        public bool CheckCombo(float offsetPercent)
        {
            bool isCombo = IsCombo(offsetPercent); 
            
            if (isCombo)
                _soundPlayer.PlayComboHit();

            UpdateStreak(isCombo);
            
            return isCombo;
        }

        private bool IsCombo(float offsetPercent) => 
            !_tower.IsEmpty && IsPermissibleOffsetPercent(offsetPercent);

        private bool IsPermissibleOffsetPercent(float percentOffset) => 
            percentOffset <= _maxComboOffsetPercent;

        private void UpdateStreak(bool isCombo)
        {
            if (isCombo)
            {
                float streakIncrement = ++Count * 0.1f;
                Streak = Mathf.Clamp(Streak + streakIncrement, 1f, 2f);
                return;
            }

            Count = 0;
            Streak = 1f;
        }
    }
}