using CodeBase.Infrastructure.Scene.Loading;
using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.States.GameStates
{
    public class LoadSceneState : IGameState, IPayloadedState<string>
    {
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly GameStateMachine _gameStateMachine;

        public LoadSceneState(
            SceneLoader sceneLoader, 
            LoadingCurtain loadingCurtain, 
            GameStateMachine gameStateMachine)
        {
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameStateMachine = gameStateMachine;
        }

        public async void Enter(string sceneName)
        {
            await _loadingCurtain.Show();
            await _sceneLoader.Load(sceneName);

            _gameStateMachine.Enter<GameplayState>();
        }

        public async void Exit()
        {
            await _loadingCurtain.Hide();
        }
    }
}