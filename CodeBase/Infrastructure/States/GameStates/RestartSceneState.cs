using CodeBase.Infrastructure.Scene.Loading;

namespace CodeBase.Infrastructure.States.GameStates
{
    public class RestartSceneState : IGameState, IState
    {
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;

        public RestartSceneState(
            SceneLoader sceneLoader, 
            LoadingCurtain loadingCurtain)
        {
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
        }

        public async void Enter()
        {
            await _loadingCurtain.Show();
            await _sceneLoader.Reload();
        }

        public async void Exit() => 
            await _loadingCurtain.Hide();
    }
}