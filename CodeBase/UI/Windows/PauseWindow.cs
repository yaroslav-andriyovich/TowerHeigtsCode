using CodeBase.Gameplay.States;
using CodeBase.Infrastructure.States.GameStates;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Windows
{
    public class PauseWindow : WindowBase
    {
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _menuButton;
        
        private LevelStateMachine _levelStateMachine;
        private GameStateMachine _gameStateMachine;

        private void Start()
        {
            _resumeButton.onClick.AddListener(OnResumeButton);
            _menuButton.onClick.AddListener(OnMenuButton);
        }

        private void OnDestroy()
        {
            _resumeButton.onClick.RemoveListener(OnResumeButton);
            _menuButton.onClick.RemoveListener(OnMenuButton);
        }

        [Inject]
        public void Construct(
            LevelStateMachine levelStateMachine,
            GameStateMachine gameStateMachine
            )
        {
            _levelStateMachine = levelStateMachine;
            _gameStateMachine = gameStateMachine;
        }

        private void OnResumeButton() => 
            _levelStateMachine.Enter<LevelLoopState>();

        private void OnMenuButton()
        {
            _levelStateMachine.Dispose();
            _gameStateMachine.Enter<RestartSceneState>();
        }
    }
}