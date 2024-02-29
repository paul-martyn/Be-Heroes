using CodeBase.Infrastructure.Factory;
using CodeBase.UI.Services.Factory;

namespace CodeBase.UI.Services.Windows
{
    public class WindowService : IWindowService
    {
        private readonly IUIFactory _uiFactory;
        private GameFactory _gameFactory;

        public WindowService(IUIFactory uiFactory) => 
            _uiFactory = uiFactory;
        
        public void Open(WindowId windowId)
        {
            switch (windowId)
            {
                case WindowId.Unknown:
                    break;
                case WindowId.Shop:
                    _uiFactory.CreateShop();
                    break;
                case WindowId.Death:
                    _uiFactory.CreateDeath();
                    break;
            }
        }
    }
}