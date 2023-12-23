using CodeBase.Data.Level;
using CodeBase.Gameplay;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/Levels", order = 1)]
    public class LevelStaticData : ScriptableObject
    {
        public GameMode _gameMode;
        [Space]
        public BlockPoolData BlockPoolData;
        [Space]
        public HoistingRopeData HoistingRopeData;
        [Space]
        public MissDetectorData _missDetectorData;
        [Space]
        public OffsetCheckerData OffsetCheckerData;
        [Space]
        public ComboCheckerData ComboCheckerData;
        [Space]
        public TowerStabilityData TowerStabilityData;
        [Space]
        public TransformDescenderData TransformDescenderData;
        [Space]
        public ReleaseTimerData ReleaseTimerData;
        [Space]
        public CameraShakerData CameraShakerData;
    }
}