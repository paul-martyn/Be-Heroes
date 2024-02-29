
using UnityEngine;

namespace CodeBase.Services.Input
{
    abstract class InputService : IInputService
    {
        protected const string Horizontal = "Horizontal";
        protected const string Vertical = "Vertical";
        
        private const string Button = "Attack";
        
        public abstract Vector2 Axis { get; }

        public bool IsAttackButtonPress() => 
            SimpleInput.GetButtonUp(Button);

        protected static Vector2 SimpleInputAxis() => 
            new(UnityEngine.Input.GetAxis(Horizontal), UnityEngine.Input.GetAxis(Vertical));
    }
}