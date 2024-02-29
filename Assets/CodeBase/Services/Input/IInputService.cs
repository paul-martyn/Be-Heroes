using Vector2 = UnityEngine.Vector2;

namespace CodeBase.Services.Input
{
    public interface IInputService : IService
    {
        Vector2 Axis { get; }
        bool IsAttackButtonPress();
    }
}