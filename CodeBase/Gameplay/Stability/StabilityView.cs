using CodeBase.UI;
using TMPro;
using UnityEngine;

namespace CodeBase.Gameplay.Stability
{
    public class StabilityView : ProgressBarView
    {
        [SerializeField] private TMP_Text _text;

        public override void SetProgress(float current, float max)
        {
            base.SetProgress(current, max);
            ChangeText();
        }

        private void ChangeText() => 
            _text.text = (_progress * 100f).ToString("F0") + "%";
    }
}