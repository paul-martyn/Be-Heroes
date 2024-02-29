using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class OpenWindowButton : MonoBehaviour
    {
        [SerializeField] private Button Button;
        [SerializeField] private WindowId WindowId;

        private IWindowService _windowService;

        public void Construct(IWindowService windowService) => 
            _windowService = windowService;

        private void Awake() => 
            Button.onClick.AddListener(Open);

        private void Open() => 
            _windowService.Open(WindowId);
    }
}