using Core.Dependency;

namespace Core.DependencyInterfaces
{
    public interface IInputService : IService
    {
        public InputSystem_Actions InputSystem { get; }
        public InputSystem_Actions.PlayerActions Player { get; }
        public InputSystem_Actions.UIActions UI { get; }
    }
}