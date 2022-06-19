using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CubeConquer.Managers
{
    public class ManagerProvider
    {
        private static ManagerProvider instance;
        private static ManagerProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ManagerProvider();
                }

                return instance;
            }
        }

        private readonly Dictionary<System.Type, IManager> managerDict = new();

        public static void AddManager<T>(IManager manager)
        {
            Instance.managerDict.Remove(typeof(T));
            Instance.managerDict.Add(typeof(T), manager);
        }

        public static T GetManager<T>()
        {
            return Instance.managerDict.ContainsKey(typeof(T)) ? (T)Instance.managerDict[typeof(T)] : default(T);
        }

        public static void RemoveManager<T>()
        {
            Instance.managerDict.Remove(typeof(T));
        }

        //private static ManagerProvider Instance;
        //private List<IManager> Managers;

        //private ManagerProvider()
        //{
        //    Managers = new List<IManager>();
        //}
        //~ManagerProvider()
        //{
        //    if (Managers != null)
        //        Managers.Clear();
        //}
        //private static ManagerProvider GetInstance()
        //{
        //    if (Instance == null)
        //    {
        //        Instance = new ManagerProvider();
        //    }

        //    return Instance;
        //}

        //public static void AddManager(IManager manager)
        //{
        //    GetInstance().Managers.Add(manager);
        //}

        //public static void RemoveManager(IManager manager)
        //{
        //    GetInstance().Managers.Remove(manager);
        //}

        //public static IManager GetManager(string ManagerType)
        //{
        //    ManagerProvider mP = GetInstance();

        //    foreach (IManager manager in mP.Managers)
        //    {
        //        if (manager.ManagerType == ManagerType)
        //            return manager;
        //    }

        //    Debug.LogError("There is no " + ManagerType + " in the Scene, add an " + ManagerType + " to the Scene");
        //    return null;
        //}

    }
}