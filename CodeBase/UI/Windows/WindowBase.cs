using CodeBase.Extensions;
using UnityEngine;

namespace CodeBase.UI.Windows
{
    public abstract class WindowBase : MonoBehaviour
    {
        public virtual void Show()
        {
            if (isActiveAndEnabled)
                return;
            
            gameObject.MakeActive();
        }

        public virtual void Hide()
        {
            if (!isActiveAndEnabled)
                return;
            
            gameObject.MakeInactive();
        }
    }
}