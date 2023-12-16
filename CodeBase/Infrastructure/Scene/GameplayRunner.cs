using CodeBase.Gameplay.States;
using CodeBase.Infrastructure.States.GameStates;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Scene
{
    public class GameplayRunner : MonoBehaviour
    {
        private GameStateMachine _gameStateMachine;
        private LevelStateMachine _levelStateMachine;

        private void Start()
        {
            _gameStateMachine.Enter<GameplayState>();
            _levelStateMachine.Enter<LevelStartState>();
        }

        [Inject]
        public void Construct(GameStateMachine gameStateMachine, LevelStateMachine levelStateMachine)
        {
            _gameStateMachine = gameStateMachine;
            _levelStateMachine = levelStateMachine;
        }
    }
}