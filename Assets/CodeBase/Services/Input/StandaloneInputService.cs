using UnityEngine;

namespace CodeBase.Services.Input
{
    class StandaloneInputService : InputService
    {
        public override Vector2 Axis
        {
            get
            {
                Vector2 axis = UnityAxis();
                if (axis == Vector2.zero)
                    axis = SimpleInputAxis();
                return axis;
            }
        }

        private static Vector2 UnityAxis() =>
            new(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));
    }
}