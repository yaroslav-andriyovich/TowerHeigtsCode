using CodeBase.UI.Windows;

namespace CodeBase.UI.Services
{
    public interface IWindowService
    {
        void Show<TWindow>() where TWindow : WindowBase;
        void Hide<TWindow>() where TWindow : WindowBase;
    }
}