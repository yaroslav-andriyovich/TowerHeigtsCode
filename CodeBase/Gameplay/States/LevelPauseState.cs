using CodeBase.Infrastructure.States;
using UnityEngine;

namespace CodeBase.Gameplay.States
{
    public class LevelPauseState : ILevelState, IState
    {
        public void Enter()
        {
            Time.timeScale = 0f;
            Debug.Log("Pause.");
        }

        public void Exit()
        {
            Time.timeScale = 1f;
            Debug.Log("Game resumed.");
        }
    }
}