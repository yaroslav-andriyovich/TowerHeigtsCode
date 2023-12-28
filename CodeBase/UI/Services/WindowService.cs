using System;
using System.Collections.Generic;
using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.UI.Services
{
    public class WindowService : IWindowService, IDisposable
    {
        private readonly WindowFactory _windowFactory;
        private readonly Dictionary<Type, WindowBase> _displayedWindows;

        protected WindowService(WindowFactory windowFactory)
        {
            _windowFactory = windowFactory;
            _displayedWindows = new Dictionary<Type, WindowBase>();
        }

        public void Dispose() => 
            _displayedWindows.Clear();

        public void Show<TWindow>() where TWindow : WindowBase
        {
            Type windowType = typeof(TWindow);
            
            if (_displayedWindows.ContainsKey(windowType))
            {
                Debug.LogError($"Window {windowType} already registered!");
                return;
            }
            
            WindowBase window = _windowFactory.GetWindow<TWindow>();
            
            window.Show();

            _displayedWindows.Add(windowType, window);
        }

        public void Hide<TWindow>() where TWindow : WindowBase
        {
            Type windowType = typeof(TWindow);

            if (!_displayedWindows.ContainsKey(windowType))
            {
                Debug.LogError($"Window {windowType} not registered!");
                return;
            }
            
            WindowBase window = _displayedWindows[windowType];
            
            window.Hide();
            
            _displayedWindows.Remove(windowType);
        }
    }
}