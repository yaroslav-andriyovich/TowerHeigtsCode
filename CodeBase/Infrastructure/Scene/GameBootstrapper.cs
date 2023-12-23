using CodeBase.Infrastructure.States.GameStates;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Scene
{
    public class GameBootstrapper : MonoBehaviour, IInitializable
    {
        private GameStateMachine _gameStateMachine;

        [Inject]
        public void Construct(GameStateMachine gameStateMachine) => 
            _gameStateMachine = gameStateMachine;

        public void Initialize() => 
            _gameStateMachine.Enter<BootstrapState>();
    }
}