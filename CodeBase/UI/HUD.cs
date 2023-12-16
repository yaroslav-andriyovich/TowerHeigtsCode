using UnityEngine;

namespace CodeBase.UI
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private UIIdlePanel _idlePanel;

        public UIIdlePanel IdlePanel => _idlePanel;

        private void Start() =>
            ShowIdlePanel();

        private void OnDisable()
        {
            _idlePanel.Clear();
        }

        public void ShowIdlePanel() => 
            _idlePanel.Show();

        public void HideIdlePanel() => 
            _idlePanel.Hide();

        public void OnButtonClick()
        {
            
        }
    }
}