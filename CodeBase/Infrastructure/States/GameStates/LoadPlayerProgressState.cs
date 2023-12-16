using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Services.PlayerProgressService;
using CodeBase.Services.SaveLoadService;

namespace CodeBase.Infrastructure.States.GameStates
{
    public class LoadPlayerProgressState : IGameState, IState
    {
        private const string MainSceneName = "Main";
        
        private readonly GameStateMachine _stateMachine;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IEnumerable<IProgressReader> _progressReaderServices;
        private readonly IPlayerProgressService _progressService;
        
        public LoadPlayerProgressState(
            GameStateMachine stateMachine, 
            IPlayerProgressService progressService, 
            ISaveLoadService saveLoadService, 
            IEnumerable<IProgressReader> progressReaderServices)
        {
            _stateMachine = stateMachine;
            _saveLoadService = saveLoadService;
            _progressService = progressService;
            _progressReaderServices = progressReaderServices;
        }

        public void Enter()
        {
            PlayerProgress progress = LoadProgressOrInitNew();
            
            NotifyProgressReaderServices(progress);
            
            _stateMachine.Enter<LoadSceneState, string>(MainSceneName);
        }

        private void NotifyProgressReaderServices(PlayerProgress progress)
        {
            foreach (IProgressReader reader in _progressReaderServices)
                reader.LoadProgress(progress);
        }

        public void Exit()
        {
        }

        private PlayerProgress LoadProgressOrInitNew() =>
            _progressService.Progress = 
                _saveLoadService.LoadProgress() 
                ?? NewProgress();

        private PlayerProgress NewProgress() => 
            new PlayerProgress();
    }
}