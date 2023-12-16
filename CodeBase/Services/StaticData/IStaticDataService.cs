using CodeBase.Gameplay;
using CodeBase.StaticData;

namespace CodeBase.Services.StaticData
{
    public interface IStaticDataService
    {
        void Load();
        LevelStaticData ForGameMode(GameMode sceneKey);
        LevelStaticData ForCurrentMode();
    }
}