using CodeBase.Gameplay;
using CodeBase.Gameplay.BaseBlock.Factory;
using CodeBase.Gameplay.BlocksPool;
using CodeBase.Gameplay.BlockTracking;
using CodeBase.Gameplay.CameraManagement;
using CodeBase.Gameplay.Combo;
using CodeBase.Gameplay.RopeManagement;
using CodeBase.Gameplay.Stability;
using CodeBase.Gameplay.States;
using CodeBase.Gameplay.TowerManagement;
using CodeBase.Gameplay.TransformDescend;
using CodeBase.UI.Services;
using CodeBase.UI.Windows;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Scene
{
    public class LevelInstaller : MonoInstaller
    {
        [Header("UI")]
        [Header("Windows")]
        [SerializeField] private PauseWindow _pauseWindow;
        [SerializeField] private GameplayWindow _gameplayWindow;
        [Header("Elements")]
        [SerializeField] private ReleaseTimerView _releaseTimerView;
        [SerializeField] private StabilityView _stabilityView;
        
        [Header("Scene Instances")]
        [Space]
        [SerializeField] private Rope _rope;
        [SerializeField] private Tower _tower;
        [SerializeField] private TransformsToDescendProvider _transformsToDescendProvider;
        [SerializeField] private CameraBlur _cameraBlur;
        
        public override void InstallBindings()
        {
            BindLevelStates();
            Container.Bind<LevelStateFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelStateMachine>().AsSingle();
            
            BindWindows();

            BindSceneInstances();
            BindGameplayServices();
        }

        private void BindLevelStates()
        {
            Container.Bind<LevelStartState>().AsSingle();
            Container.Bind<LevelLoopState>().AsSingle();
            Container.Bind<LevelPauseState>().AsSingle();
            Container.Bind<LevelFailState>().AsSingle();
        }

        private void BindWindows()
        {
            Container.Bind<PauseWindow>().FromInstance(_pauseWindow).AsSingle();
            Container.Bind<GameplayWindow>().FromInstance(_gameplayWindow).AsSingle();
            
            Container.Bind<WindowFactory>().AsSingle();
            Container.BindInterfacesTo<WindowService>().AsSingle();
        }

        private void BindSceneInstances()
        {
            Container.Bind<Rope>().FromInstance(_rope).AsSingle();
            Container.Bind<Tower>().FromInstance(_tower).AsSingle();
            Container.Bind<TowerStability>().FromComponentOn(_tower.gameObject).AsSingle();
            Container.Bind<TowerCollapse>().FromComponentOn(_tower.gameObject).AsSingle();
            Container.Bind<TransformsToDescendProvider>().FromInstance(_transformsToDescendProvider).AsSingle();
            Container.Bind<ReleaseTimerView>().FromInstance(_releaseTimerView).AsSingle();
            Container.Bind<StabilityView>().FromInstance(_stabilityView).AsSingle();
            Container.Bind<CameraBlur>().FromInstance(_cameraBlur).AsSingle();
        }

        private void BindGameplayServices()
        {
            Container.BindInterfacesTo<BlockFactory>().AsSingle();
            Container.BindInterfacesTo<BlockPool>().AsSingle();
            Container.Bind<BlockCombiner>().AsSingle();

            Container.Bind<Timer>().AsTransient();
            Container.BindInterfacesAndSelfTo<ReleaseTimer>().AsSingle();
            Container.Bind<RopeAttachment>().AsSingle();

            Container.BindInterfacesAndSelfTo<CollisionDetector>().AsSingle();
            Container.Bind<ObstacleValidator>().AsSingle();
            Container.BindInterfacesAndSelfTo<MissDetector>().AsSingle();
            Container.BindInterfacesAndSelfTo<BlockTracker>().AsSingle();

            Container.BindInterfacesAndSelfTo<OffsetChecker>().AsSingle();
            Container.BindInterfacesAndSelfTo<CameraShaker>().AsSingle();
            
            Container.Bind<BlockLanding>().AsSingle();
            Container.BindInterfacesAndSelfTo<ComboSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<CollisionHandler>().AsSingle();

            Container.BindInterfacesAndSelfTo<TransformDescender>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<GameFlow>().AsSingle();
        }
    }
}