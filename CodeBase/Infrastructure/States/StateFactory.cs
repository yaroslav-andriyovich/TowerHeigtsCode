using System;
using System.Collections.Generic;
using Zenject;

namespace CodeBase.Infrastructure.States
{
    public abstract class StateFactory : IFactory<Type, IExitableState>
    {
        private readonly Dictionary<Type, Func<IExitableState>> _states;

        protected StateFactory(DiContainer container) => 
            _states = BuildStatesRegister(container);

        public IExitableState Create(Type type)
        {
            Func<IExitableState> func;
            if (!_states.TryGetValue(type, out func))
            {
                throw new Exception("State " + type.Name + " is not registered!");
            }

            return func();
        }

        public T GetState<T>() where T : class, IExitableState => 
            Create(typeof(T)) as T;

        protected abstract Dictionary<Type, Func<IExitableState>> BuildStatesRegister(DiContainer container);
    }
}