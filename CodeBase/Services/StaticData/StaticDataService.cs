using System.Collections.Generic;
using System.Linq;
using CodeBase.Gameplay;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.StaticData;

namespace CodeBase.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string LevelConfigsPath = "StaticData/Levels";
        
        private readonly IAssetProvider _assetProvider;
        
        private Dictionary<GameMode, LevelStaticData> _levels;

        public StaticDataService(IAssetProvider assetProvider) => 
            _assetProvider = assetProvider;

        public void Load()
        {
            _levels = _assetProvider
                .LoadAll<LevelStaticData>(LevelConfigsPath)
                .ToDictionary(x => x._gameMode, x => x);
        }

        public LevelStaticData ForGameMode(GameMode gameMode) => 
            _levels.TryGetValue(gameMode, out LevelStaticData staticData) 
                ? staticData 
                : null;
        
        public LevelStaticData ForCurrentMode() => 
            ForGameMode(GameMode.Test);
            //ForGameMode(GameMode.Main);
    }
}