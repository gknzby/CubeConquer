
namespace CubeConquer.Components
{
    public interface IUIMenu
    {
        void ShowMenu();
        void HideMenu();
        string GetMenuName();
        void RegisterToUIManager();
    }
}
