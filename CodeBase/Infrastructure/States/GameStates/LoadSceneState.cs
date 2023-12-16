using CodeBase.Infrastructure.Scene.Loading;

namespace CodeBase.Infrastructure.States.GameStates
{
    public class LoadSceneState : IGameState, IPayloadedState<string>
    {
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;

        public LoadSceneState(SceneLoader sceneLoader, LoadingCurtain loadingCurtain)
        {
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
        }

        public async void Enter(string sceneName)
        {
            await _loadingCurtain.Show();
            await _sceneLoader.Load(sceneName);
        }

        public async void Exit() => 
            await _loadingCurtain.Hide();
    }
}