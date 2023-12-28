using System;
using System.Collections.Generic;
using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.UI.Services
{
    public class WindowService : IWindowService, IDisposable
    {
        private readonly WindowFactory _windowFactory;
        private readonly Dictionary<Type, WindowBase> _openedWindows;

        protected WindowService(WindowFactory windowFactory)
        {
            _windowFactory = windowFactory;
            _openedWindows = new Dictionary<Type, WindowBase>();
        }

        public void Dispose() => 
            _openedWindows.Clear();

        public void Open<TWindow>() where TWindow : WindowBase
        {
            Type windowType = typeof(TWindow);
            
            if (_openedWindows.ContainsKey(windowType))
            {
                Debug.LogError($"Window {windowType} already registered!");
                return;
            }
            
            WindowBase window = _windowFactory.GetWindow<TWindow>();
            
            window.Show();

            _openedWindows.Add(windowType, window);
        }

        public void Close<TWindow>() where TWindow : WindowBase
        {
            Type windowType = typeof(TWindow);

            if (!_openedWindows.ContainsKey(windowType))
            {
                Debug.LogError($"Window {windowType} not registered!");
                return;
            }
            
            WindowBase window = _openedWindows[windowType];
            
            window.Hide();
            
            _openedWindows.Remove(typeof(TWindow));
        }
    }
}