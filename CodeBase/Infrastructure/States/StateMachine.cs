using System;
using Zenject;

namespace CodeBase.Infrastructure.States
{
    public abstract class StateMachine<TBaseState> : ITickable, IDisposable
    {
        private readonly StateFactory _stateFactory;
        
        private IExitableState _activeState;
        private ITickable _activeTickableState;

        protected StateMachine(StateFactory stateFactory) => 
            _stateFactory = stateFactory;

        public void Tick() => 
            _activeTickableState?.Tick();

        public void Dispose()
        {
            if (_activeState != null)
                _activeState.Exit();
            
            _activeState = null;
            _activeTickableState = null;
        }
        
        public bool InState<TState>() where TState : class, TBaseState, IExitableState => 
            _activeState.GetType() == typeof(TState);

        public void Enter<TState>() where TState : class, TBaseState, IState
        {
            IState state = ChangeState<TState>();
            
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, TBaseState, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            TState state = _stateFactory.GetState<TState>();
            
            _activeState = state;
            _activeTickableState = state as ITickable;

            return state;
        }
    }
}