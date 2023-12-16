using CodeBase.Infrastructure.Scene.Loading;
using CodeBase.Services.StaticData;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Infrastructure.States.GameStates
{
    public class BootstrapState : IGameState, IState
    {
        private const string MainSceneName = "Main";
        
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IStaticDataService _staticDataService;

        public BootstrapState(
            GameStateMachine stateMachine, 
            SceneLoader sceneLoader,
            IStaticDataService staticDataService
            )
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _staticDataService = staticDataService;
        }

        public async void Enter()
        {
            InitializeApplication();
            LoadStaticData();

            await _sceneLoader.Load(MainSceneName);
            LoadProgress();
        }

        public void Exit()
        {
        }

        private void InitializeApplication()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            DOTween.Init();
        }

        private void LoadStaticData() => 
            _staticDataService.Load();

        private void LoadProgress() => 
            _stateMachine.Enter<LoadPlayerProgressState>();
    }
}