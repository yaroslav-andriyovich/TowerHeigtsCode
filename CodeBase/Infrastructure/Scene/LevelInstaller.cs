using CodeBase.Gameplay.BaseBlock.Factory;
using CodeBase.Gameplay.BlocksPool;
using CodeBase.Gameplay.HoistingRopeLogic;
using CodeBase.Gameplay.Services;
using CodeBase.Gameplay.Services.BlockBind;
using CodeBase.Gameplay.Services.BlockMiss;
using CodeBase.Gameplay.Services.BlockReleaseTimer;
using CodeBase.Gameplay.Services.Collision;
using CodeBase.Gameplay.Services.Combo;
using CodeBase.Gameplay.Services.TransformDescend;
using CodeBase.Gameplay.Stability;
using CodeBase.Gameplay.States;
using CodeBase.Gameplay.TowerLogic;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Scene
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private HoistingRope _hoistingRope;
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
            Container.Bind<HoistingRope>().FromInstance(_hoistingRope).AsSingle();
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
            Container.Bind<ReleasedBlockTracker>().AsSingle();
            Container.Bind<BlockBinder>().AsSingle();
            Container.Bind<Timer>().AsTransient();
            Container.BindInterfacesAndSelfTo<ReleaseTimerController>().AsSingle();
            Container.BindInterfacesAndSelfTo<MissChecker>().AsSingle();
            Container.BindInterfacesAndSelfTo<OffsetChecker>().AsSingle();
            Container.BindInterfacesAndSelfTo<ComboChecker>().AsSingle();
            Container.Bind<ObstacleValidator>().AsSingle();
            Container.Bind<CollisionValidator>().AsSingle();
            Container.BindInterfacesAndSelfTo<CollisionObserver>().AsSingle();
            Container.Bind<LandingController>().AsSingle();
            Container.BindInterfacesAndSelfTo<TransformDescender>().AsSingle();
            Container.BindInterfacesAndSelfTo<StabilityController>().AsSingle();
            
            Container.Bind<CollisionHandler>().AsSingle();
            Container.Bind<BlockHandler>().AsSingle();
        }
    }
}