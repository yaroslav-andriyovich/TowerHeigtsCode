using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Services.PlayerProgressService;
using UnityEngine;

namespace CodeBase.Services.SaveLoadService
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string ProgressKey = "Progress";
        
        private readonly IEnumerable<IProgressSaver> _saverService;
        private readonly IPlayerProgressService _playerProgressService;

        public SaveLoadService(IEnumerable<IProgressSaver> saverService, IPlayerProgressService playerProgressService)
        {
            _saverService = saverService;
            _playerProgressService = playerProgressService;
        }

        public void SaveProgress()
        {
            foreach (var saver in _saverService) 
                saver.UpdateProgress(_playerProgressService.Progress);
            
            PlayerPrefs.SetString(ProgressKey, _playerProgressService.Progress.ToJson());
        }

        public PlayerProgress LoadProgress() => 
            PlayerPrefs.
                GetString(ProgressKey)?.
                ToDeserialized<PlayerProgress>();
    }
}