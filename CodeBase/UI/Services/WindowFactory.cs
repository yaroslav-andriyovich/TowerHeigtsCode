using System;
using System.Collections.Generic;
using CodeBase.UI.Windows;
using Zenject;

namespace CodeBase.UI.Services
{
    public class WindowFactory : IFactory<Type, WindowBase>
    {
        private readonly Dictionary<Type, Func<WindowBase>> _windows;

        protected WindowFactory(DiContainer container) => 
            _windows = BuildWindowsRegister(container);

        public WindowBase Create(Type type)
        {
            Func<WindowBase> func;
            if (!_windows.TryGetValue(type, out func))
            {
                throw new Exception("Window " + type.Name + " is not registered!");
            }

            return func();
        }

        public T GetWindow<T>() where T : WindowBase => 
            Create(typeof(T)) as T;

        protected Dictionary<Type, Func<WindowBase>> BuildWindowsRegister(DiContainer container)
        {
            Dictionary<Type, Func<WindowBase>> windows = new Dictionary<Type, Func<WindowBase>>()
            {
                [typeof(GameplayWindow)] = container.Resolve<GameplayWindow>,
                [typeof(PauseWindow)] = container.Resolve<PauseWindow>,
            };

            return windows;
        }
    }
}