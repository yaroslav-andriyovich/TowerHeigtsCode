using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Scene;
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
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeReference] private GameBootstrapper _gameBootstrapper;
        [SerializeReference] private LoadingCurtain _loadingCurtain;
        [SerializeReference] private SoundPlayer _soundPlayer;

        public override void InstallBindings()
        {
            BindLoadServices();
            BindGameStateMachine();
            BindDataServices();
            BindInputService();
            BindSoundService();
        }

        private void BindLoadServices()
        {
            Container.Bind<LoadingCurtain>().FromComponentInNewPrefab(_loadingCurtain).AsSingle();
            Container.Bind<SceneLoader>().AsSingle();
            Container.BindInterfacesTo<AssetProvider>().AsSingle();
        }

        private void BindGameStateMachine()
        {
            Container.Bind<GameStateFactory>().AsSingle();
            BindGameStates();
            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle();
            Container.BindInterfacesTo<GameBootstrapper>().FromComponentInNewPrefab(_gameBootstrapper).AsSingle();
        }

        private void BindGameStates()
        {
            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<LoadSceneState>().AsSingle();
            Container.Bind<RestartSceneState>().AsSingle();
            Container.Bind<LoadPlayerProgressState>().AsSingle();
            Container.Bind<GameplayState>().AsSingle();
        }

        private void BindDataServices()
        {
            Container.BindInterfacesTo<PlayerProgressService>().AsSingle();
            Container.BindInterfacesTo<SaveLoadService>().AsSingle();
            Container.BindInterfacesTo<StaticDataService>().AsSingle();
        }

        private void BindInputService() => 
            Container.Bind<InputActions>().AsSingle();

        private void BindSoundService() => 
            Container.Bind<SoundPlayer>().FromComponentInNewPrefab(_soundPlayer).AsSingle();
    }
}