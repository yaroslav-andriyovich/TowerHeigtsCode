using CodeBase.Gameplay.States;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Scene
{
    public class GameplayRunner : MonoBehaviour
    {
        private LevelStateMachine _levelStateMachine;

        private void Start() => 
            _levelStateMachine.Enter<LevelStartState>();

        [Inject]
        public void Construct(LevelStateMachine levelStateMachine) => 
            _levelStateMachine = levelStateMachine;
    }
}