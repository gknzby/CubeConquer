using CubeConquer.Managers;
using UnityEngine;

namespace CubeConquer.Components
{
    public class UIComponent : MonoBehaviour
    {
        public UIAction uiAction;
        public string value;

        public void SendToUIManager()
        {
            ManagerProvider.GetManager<IUIManager>().SendUIComponent(this);
        }
    }
}