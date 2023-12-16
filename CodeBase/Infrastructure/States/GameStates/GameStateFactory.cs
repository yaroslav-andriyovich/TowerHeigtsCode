using System;
using System.Collections.Generic;
using Zenject;

namespace CodeBase.Infrastructure.States.GameStates
{
    public class GameStateFactory : StateFactory
    {
        public GameStateFactory(DiContainer container) : base(container)
        {
        }

        protected override Dictionary<Type, Func<IExitableState>> BuildStatesRegister(DiContainer container)
        {
            Dictionary<Type, Func<IExitableState>> states = new Dictionary<Type, Func<IExitableState>>()
            {
                [typeof(BootstrapState)] = container.Resolve<BootstrapState>,
                [typeof(LoadSceneState)] = container.Resolve<LoadSceneState>,
                [typeof(RestartSceneState)] = container.Resolve<RestartSceneState>,
                [typeof(LoadPlayerProgressState)] = container.Resolve<LoadPlayerProgressState>,
                [typeof(GameplayState)] = container.Resolve<GameplayState>,
            };

            return states;
        }
    }
}