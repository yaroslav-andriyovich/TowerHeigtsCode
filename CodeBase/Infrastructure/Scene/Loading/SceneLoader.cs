using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.Scene.Loading
{
    public class SceneLoader
    {
        public async UniTask Load(string sceneName)
        {
            if (SceneManager.GetActiveScene().name == sceneName)
                return;

            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(sceneName);
            
            await UniTask.WaitUntil(() => waitNextScene.isDone);
        }

        public async UniTask Reload()
        {
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            
            await UniTask.WaitUntil(() => waitNextScene.isDone);
        }
    }
}