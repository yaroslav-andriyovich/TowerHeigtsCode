using CodeBase.Infrastructure.Scene.Loading;

namespace CodeBase.Infrastructure.States.GameStates
{
    public class RestartSceneState : IGameState, IState
    {
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly GameStateMachine _gameStateMachine;

        public RestartSceneState(
            SceneLoader sceneLoader, 
            LoadingCurtain loadingCurtain,
            GameStateMachine gameStateMachine
            )
        {
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameStateMachine = gameStateMachine;
        }

        public async void Enter()
        {
            await _loadingCurtain.Show();
            await _sceneLoader.Reload();
            
            _gameStateMachine.Enter<GameplayState>();
        }

        public async void Exit()
        {
            await _loadingCurtain.Hide();
        }
    }
}