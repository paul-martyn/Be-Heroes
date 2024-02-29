using CodeBase.Infrastructure.States;
using CodeBase.Services;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public class DeathWindow : WindowBase
    {
        [SerializeField] private Button _restartButton;
        
        protected override void Initialize() => 
            _restartButton.onClick.AddListener(Restart);

        protected override void Cleanup()
        {
            base.Cleanup();
            _restartButton.onClick.RemoveListener(Restart);
        }

        private void Restart() => 
            AllServices.Container.Single<IGameStateMachine>().Enter<BootstrapState>();
    }
}
