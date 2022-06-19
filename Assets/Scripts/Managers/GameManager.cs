using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeConquer.Managers
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        public void TestFunc(string testStr)
        {
            Debug.Log(testStr);
        }

        private void Awake()
        {
            ManagerProvider.AddManager<IGameManager>(this);
        }

        private void Start()
        {
            ManagerProvider.GetManager<IInputManager>().StartSendingInputs();
        }
        private void OnDestroy()
        {
            ManagerProvider.RemoveManager<IGameManager>();
        }
    }
}
