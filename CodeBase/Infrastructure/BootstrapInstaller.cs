using System.Collections;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Scene.Loading;
using CodeBase.Infrastructure.States.GameStates;
using CodeBase.Services.PlayerProgressService;
using CodeBase.Services.SaveLoadService;
using CodeBase.Services.StaticData;
using CodeBase.Sounds;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class BootstrapInstaller : MonoInstaller, IInitializable, ICoroutineRunner
    {
        [SerializeField] private LoadingCurtain _loadingCurtain;
        [SerializeField] private SoundPlayer _soundPlayer;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<BootstrapInstaller>().FromInstance(this);

            Container.Bind<LoadingCurtain>().FromComponentInNewPrefab(_loadingCurtain).AsSingle();
            Container.Bind<SceneLoader>().AsSingle();
            Container.Bind<SceneReadyObserver>().AsSingle();

            Container.Bind<GameStateFactory>().AsSingle();
            BindGameStates();
            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle();

            Container.BindInterfacesTo<AssetProvider>().AsSingle();
            Container.BindInterfacesTo<PlayerProgressService>().AsSingle();
            Container.BindInterfacesTo<SaveLoadService>().AsSingle();
            Container.BindInterfacesTo<StaticDataService>().AsSingle();
            Container.Bind<InputActions>().AsSingle();

            Container.Bind<SoundPlayer>().FromComponentInNewPrefab(_soundPlayer).AsSingle();
        }

        void IInitializable.Initialize() => 
            Container.Resolve<GameStateMachine>().Enter<BootstrapState>();

        Coroutine ICoroutineRunner.StartCoroutine(IEnumerator routine) => 
            StartCoroutine(routine);

        void ICoroutineRunner.StopCoroutine(Coroutine coroutine) => 
            StopCoroutine(coroutine);

        private void BindGameStates()
        {
            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<LoadSceneState>().AsSingle();
            Container.Bind<RestartSceneState>().AsSingle();
            Container.Bind<LoadPlayerProgressState>().AsSingle();
            Container.Bind<GameplayState>().AsSingle();
        }
    }
}