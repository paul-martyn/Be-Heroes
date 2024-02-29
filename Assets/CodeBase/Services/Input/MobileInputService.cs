using UnityEngine;

namespace CodeBase.Services.Input
{
    class MobileInputService : InputService
    {
        public override Vector2 Axis => SimpleInputAxis();
    }
}