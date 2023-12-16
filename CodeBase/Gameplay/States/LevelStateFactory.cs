using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.States;
using Zenject;

namespace CodeBase.Gameplay.States
{
    public class LevelStateFactory : StateFactory
    {
        public LevelStateFactory(DiContainer container) : base(container)
        {
        }

        protected override Dictionary<Type, Func<IExitableState>> BuildStatesRegister(DiContainer container)
        {
            Dictionary<Type, Func<IExitableState>> states = new Dictionary<Type, Func<IExitableState>>()
            {
                [typeof(LevelStartState)] = container.Resolve<LevelStartState>,
                [typeof(LevelLoopState)] = container.Resolve<LevelLoopState>,
                [typeof(LevelPauseState)] = container.Resolve<LevelPauseState>,
                [typeof(LevelFailState)] = container.Resolve<LevelFailState>,
            };

            return states;
        }
    }
}