using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.Scene.Loading
{
    public class SceneLoader
    {
        private readonly SceneReadyObserver _sceneReadyObserver;

        public SceneLoader(SceneReadyObserver sceneReadyObserver) => 
            _sceneReadyObserver = sceneReadyObserver;

        public async UniTask Load(string sceneName)
        {
            _sceneReadyObserver.MarkLoadStart();
            
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                _sceneReadyObserver.MarkReady();
                return;
            }

            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(sceneName);
            
            await UniTask.WaitUntil(() => waitNextScene.isDone);
            _sceneReadyObserver.MarkReady();
        }

        public async UniTask Reload()
        {
            _sceneReadyObserver.MarkLoadStart();
            
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            
            await UniTask.WaitUntil(() => waitNextScene.isDone);
            _sceneReadyObserver.MarkReady();
        }
    }
}