using CodeBase.UI.Windows;

namespace CodeBase.UI.Services
{
    public interface IWindowService
    {
        void Open<TWindow>() where TWindow : WindowBase;
        void Close<TWindow>() where TWindow : WindowBase;
    }
}