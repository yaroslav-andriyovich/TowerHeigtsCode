using UnityEngine;
using Zenject;

namespace CodeBase.UI
{
    public class HUDInstaller : MonoInstaller
    {
        [SerializeField] private HUD _hudInstance;

        public override void InstallBindings()
        {
            Container.
                Bind<HUD>().
                FromInstance(_hudInstance).
                AsSingle();
        }
    }
}