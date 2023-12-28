using CodeBase.Gameplay.States;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Windows
{
    public class GameplayWindow : WindowBase
    {
        [SerializeField] private Button _pauseButton;
        
        private LevelStateMachine _levelStateMachine;

        private void Start()
        {
            _pauseButton.onClick.AddListener(OnPauseButton);
        }

        private void OnDestroy()
        {
            _pauseButton.onClick.RemoveListener(OnPauseButton);
        }

        [Inject]
        public void Construct(
            LevelStateMachine levelStateMachine
        )
        {
            _levelStateMachine = levelStateMachine;
        }

        private void OnPauseButton() => 
            _levelStateMachine.Enter<LevelPauseState>();
    }
}