using CodeBase.Gameplay;
using CodeBase.Gameplay.BaseBlock.Factory;
using CodeBase.Gameplay.BlocksPool;
using CodeBase.Gameplay.BlockTracking;
using CodeBase.Gameplay.Combo;
using CodeBase.Gameplay.RopeManagement;
using CodeBase.Gameplay.Stability;
using CodeBase.Gameplay.States;
using CodeBase.Gameplay.TowerManagement;
using CodeBase.Gameplay.TransformDescend;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Scene
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private Rope _rope;
        [SerializeField] private Tower _tower;
        [SerializeField] private TransformsToDescendProvider _transformsToDescendProvider;
        [SerializeField] private ReleaseTimerView _releaseTimerView;
        [SerializeField] private StabilityView _stabilityView;
        [SerializeField] private CameraShaker _cameraShaker;
        
        public override void InstallBindings()
        {
            BindLevelStates();
            Container.Bind<LevelStateFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelStateMachine>().AsSingle();

            BindCoreObjects();
            BindGameplayServices();
        }

        private void BindLevelStates()
        {
            Container.Bind<LevelStartState>().AsSingle();
            Container.Bind<LevelLoopState>().AsSingle();
            Container.Bind<LevelPauseState>().AsSingle();
            Container.Bind<LevelFailState>().AsSingle();
        }

        private void BindCoreObjects()
        {
            Container.Bind<Rope>().FromInstance(_rope).AsSingle();
            Container.Bind<Tower>().FromInstance(_tower).AsSingle();
            Container.Bind<TransformsToDescendProvider>().FromInstance(_transformsToDescendProvider).AsSingle();
            Container.Bind<ReleaseTimerView>().FromInstance(_releaseTimerView).AsSingle();
            Container.Bind<StabilityView>().FromInstance(_stabilityView).AsSingle();
            Container.Bind<CameraShaker>().FromInstance(_cameraShaker).AsSingle();
        }
        
        private void BindGameplayServices()
        {
            Container.BindInterfacesTo<BlockFactory>().AsSingle();
            Container.BindInterfacesTo<BlockPool>().AsSingle();

            Container.Bind<Timer>().AsTransient();
            Container.BindInterfacesAndSelfTo<ReleaseTimer>().AsSingle();
            Container.Bind<RopeAttachment>().AsSingle();

            Container.BindInterfacesAndSelfTo<CollisionDetector>().AsSingle();
            Container.Bind<ObstacleValidator>().AsSingle();
            Container.BindInterfacesAndSelfTo<MissChecker>().AsSingle();
            Container.BindInterfacesAndSelfTo<BlockTracker>().AsSingle();

            Container.BindInterfacesAndSelfTo<OffsetChecker>().AsSingle();
            Container.BindInterfacesAndSelfTo<TowerStability>().AsSingle();
            Container.Bind<BlockLanding>().AsSingle();
            Container.BindInterfacesAndSelfTo<ComboSystem>().AsSingle();
            Container.Bind<CollisionHandler>().AsSingle();

            Container.Bind<CollisionValidator>().AsSingle();
            Container.BindInterfacesAndSelfTo<TransformDescender>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<GameFlow>().AsSingle();
        }
    }
}