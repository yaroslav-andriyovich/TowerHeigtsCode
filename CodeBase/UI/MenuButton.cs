using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Button))]
    public class MenuButton : MonoBehaviour
    {
        [SerializeField] private string payload;
        
        private Button _button;

        private void Awake() => 
            _button = GetComponent<Button>();

        private void OnEnable() => 
            _button.onClick.AddListener(OnButtonPress);

        private void OnDisable() => 
            _button.onClick.RemoveListener(OnButtonPress);

        [Inject]
        private void Construct()
        {
        }

        public void OnButtonPress()
        {
            Debug.Log($"Button {payload}");
        }
    }
}