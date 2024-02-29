using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPath = "UI_Root";
        
        private Transform _uiRoot;

        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService ;

        public UIFactory(IAssetProvider assetProvider, IStaticDataService staticData, IPersistentProgressService progressService)
        {
            _assetProvider = assetProvider;
            _staticData = staticData;
            _progressService = progressService;
        }

        public async Task CreateUIRoot()
        {
            GameObject root = await _assetProvider.Instantiate(UIRootPath);
            _uiRoot = root.transform;
        }

        public void CreateShop()
        {
            WindowConfig config = _staticData.ForWindow(WindowId.Shop);
            WindowBase windowBase = Object.Instantiate(config.Prefab, _uiRoot);
            windowBase.Construct(_progressService);
        }

        public void CreateDeath()
        {
            WindowConfig config = _staticData.ForWindow(WindowId.Death);
            WindowBase windowBase = Object.Instantiate(config.Prefab, _uiRoot);
            windowBase.Construct(_progressService);
        }
    }
}