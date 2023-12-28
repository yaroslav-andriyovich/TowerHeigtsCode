using CodeBase.Gameplay.CameraManagement;
using CodeBase.Infrastructure.States;
using CodeBase.UI.Services;
using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.Gameplay.States
{
    public class LevelPauseState : ILevelState, IState
    {
        private readonly CameraBlur _cameraBlur;
        private readonly IWindowService _windowService;

        public LevelPauseState(
            CameraBlur cameraBlur,
            IWindowService windowService
            )
        {
            _cameraBlur = cameraBlur;
            _windowService = windowService;
        }
        
        public void Enter()
        {
            Time.timeScale = 0f;
            _cameraBlur.FastBlur();
            _windowService.Open<PauseWindow>();
            Debug.Log("Pause.");
        }

        public void Exit()
        {
            Time.timeScale = 1f;
            _cameraBlur.FastFocus();
            _windowService.Close<PauseWindow>();
            Debug.Log("Game resumed.");
        }
    }
}