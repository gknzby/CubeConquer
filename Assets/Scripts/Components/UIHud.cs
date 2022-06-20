using CubeConquer.Managers;
using CubeConquer.Scriptables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeConquer.Components
{
    public class UIHud : UIMenu, INewLevelListener
    {
        [SerializeField] private Transform LevelNameTransform;
        [SerializeField] private Transform PlaceCountTransform;

        private void Start()
        {
            RegisterToUIManager();
            AddSelfToTheList();
        }

        private void OnDestroy()
        {
            RemoveSelfFromTheList();
        }

        public void AddSelfToTheList()
        {
            ManagerProvider.GetManager<ILevelManager>()?.AddNewLevelListener(this);
        }

        public void RemoveSelfFromTheList()
        {
            ManagerProvider.GetManager<ILevelManager>()?.RemoveNewLevelListener(this);
        }

        public void NewLevelData(LevelData levelData)
        {
            LevelNameTransform.GetComponent<TMPro.TextMeshProUGUI>().text = levelData.LevelName;
            PlaceCountTransform.GetComponent<TMPro.TextMeshProUGUI>().text = levelData.PlaceableCount.ToString();
        }


    }
}
