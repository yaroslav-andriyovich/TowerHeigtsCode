using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.Scene.Loading
{
    public class SceneReadyObserver
    {
        private UniTaskCompletionSource _readyCompletionSource;

        public void MarkLoadStart() => 
            _readyCompletionSource = new UniTaskCompletionSource();

        public void MarkReady() => 
            _readyCompletionSource.TrySetResult();

        public UniTask WaitReady() => 
            _readyCompletionSource.Task;
    }
}