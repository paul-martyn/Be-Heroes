using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;

        private const string InitialSceneName = "Initial";
        
        public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, AllServices services)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.Load(name: InitialSceneName, onLoaded: EnterLoadLevel);
        }

        public void Exit() { }

        private void RegisterServices()
        {
            IRandomService randomService = new RandomService();
            
            IInputService inputService = RegisterInputService();
            IAssetProvider assetProvider = RegisterAssetProvider();
            IStaticDataService staticData = RegisterStaticData();
            IPersistentProgressService progressService = RegisterProgressService();
            IUIFactory uiFactory = RegisterUIFactory(assetProvider, staticData, progressService);

            _services.RegisterSingle<IGameStateMachine>(_gameStateMachine);
            IWindowService windowService = RegisterWindowService(uiFactory);
            IGameFactory gameFactory = RegisterGameFactory(assetProvider, staticData, randomService, progressService, windowService, inputService);
            ISaveLoadService saveLoadService = RegisterSaveLoadService(progressService, gameFactory);
        }
        
        private ISaveLoadService RegisterSaveLoadService(IPersistentProgressService progressService, IGameFactory gameFactory)
        { 
            _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(progressService, gameFactory));
            return _services.Single<ISaveLoadService>();
        }
        
        private IGameFactory RegisterGameFactory(IAssetProvider assetProvider, IStaticDataService staticData, IRandomService randomService, 
            IPersistentProgressService progressService, IWindowService windowService, IInputService inputService)
        { 
            _services.RegisterSingle<IGameFactory>(new GameFactory(assetProvider, staticData, randomService, progressService, windowService, inputService));
            return _services.Single<IGameFactory>();
        }
        
        private IWindowService RegisterWindowService(IUIFactory uiFactory)
        { 
            _services.RegisterSingle<IWindowService>(new WindowService(uiFactory));
            return _services.Single<IWindowService>();
        }
        
        private IUIFactory RegisterUIFactory(IAssetProvider assetProvider, IStaticDataService staticData, IPersistentProgressService progressService)
        {
            _services.RegisterSingle<IUIFactory>(new UIFactory(assetProvider, staticData, progressService));
            return _services.Single<IUIFactory>();
        }

        private IPersistentProgressService RegisterProgressService()
        {
            _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            return _services.Single<IPersistentProgressService>();
        }

        private IStaticDataService RegisterStaticData()
        {
            StaticDataService staticData = new StaticDataService();
            staticData.Load();
            _services.RegisterSingle<IStaticDataService>(staticData);
            return _services.Single<IStaticDataService>();
        }

        private void EnterLoadLevel() => 
            _gameStateMachine.Enter<LoadProgressState>();

        private IAssetProvider RegisterAssetProvider()
        { 
            _services.RegisterSingle<IAssetProvider>(new AssetProvider());
            return _services.Single<IAssetProvider>();
        }

        private IInputService RegisterInputService()
        {
            _services.RegisterSingle<IInputService>(Application.isEditor ? new StandaloneInputService() : new MobileInputService());
            return _services.Single<IInputService>();
        }
    }
}