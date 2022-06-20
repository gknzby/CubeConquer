namespace CubeConquer.Managers
{
    public interface IUIManager : IManager
    {
        void SendUIComponent(Components.UIComponent uiComponent);
        void SendUIAction(UIAction uiAction);
        void RegisterMenu(Components.IUIMenu uiMenu);
        void ShowMenu(string menuName);
        void HideMenu(string menuName);
    }
}