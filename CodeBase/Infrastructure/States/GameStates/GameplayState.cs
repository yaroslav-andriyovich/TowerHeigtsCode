namespace CodeBase.Infrastructure.States.GameStates
{
    public class GameplayState : IGameState, IState
    {
        private readonly GameStateMachine _stateMachine;

        public GameplayState(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        
        public void Enter()
        {
        }

        public void Exit()
        {
        }
    }
}