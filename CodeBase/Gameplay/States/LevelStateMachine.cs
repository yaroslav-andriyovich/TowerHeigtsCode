using CodeBase.Infrastructure.States;

namespace CodeBase.Gameplay.States
{
    public class LevelStateMachine : StateMachine<ILevelState>
    {
        public LevelStateMachine(LevelStateFactory stateFactory) : base(stateFactory)
        {
        }
    }
}