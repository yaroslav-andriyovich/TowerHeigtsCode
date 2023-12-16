namespace CodeBase.Infrastructure.States.GameStates
{
    public class GameStateMachine : StateMachine<IGameState>
    {
        public GameStateMachine(GameStateFactory stateFactory) : base(stateFactory)
        {
        }
    }
}