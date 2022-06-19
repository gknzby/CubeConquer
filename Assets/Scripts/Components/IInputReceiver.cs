namespace CubeConquer.Components
{
    public interface IInputReceiver
    {
        void Click();
        void Release();
        void Drag(UnityEngine.Vector2 dragVec);
        void Cancel();
    }
}
