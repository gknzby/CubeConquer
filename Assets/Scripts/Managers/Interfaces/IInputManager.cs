using CubeConquer.Components;
namespace CubeConquer.Managers
{
    public interface IInputManager : IManager
    {
        void SetDefaultReceiver(IInputReceiver inputReceiver);
        void RemoveDefaultReceiver(IInputReceiver inputReceiver);
        void StopSendingInputs();
        void StartSendingInputs();
    }
}
