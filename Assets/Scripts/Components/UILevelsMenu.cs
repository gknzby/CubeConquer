using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CubeConquer.Managers;

namespace CubeConquer.Components
{
    /*
     * Dependency notes: Calls ILevelManager to get Level Count
     */
    public class UILevelsMenu : UIMenu
    {
        [SerializeField] private GameObject LevelBtnPrefab;
        [SerializeField] private Transform GridTransform;

        private List<GameObject> generatedLevelButtons;

        #region UIMenu
        public override void ShowMenu()
        {
            base.ShowMenu();

            ClearLevelButtons();
            GenerateLevelButtons();
        }
        public override void HideMenu()
        {
            base.HideMenu();

            ClearLevelButtons();
        }
        #endregion

        #region Class Functions
        private void ClearLevelButtons()
        {
            if (generatedLevelButtons == null)
                return;

            foreach (GameObject levelButton in generatedLevelButtons)
            {
                Destroy(levelButton);
            }

            generatedLevelButtons.Clear();
        }

        private void GenerateLevelButtons()
        {
            int levelCount = ManagerProvider.GetManager<ILevelManager>().LevelCount;

            generatedLevelButtons = new List<GameObject>();

            for (int i = 0; i < levelCount; i++)
            {
                GameObject levelBtn = Instantiate(LevelBtnPrefab, GridTransform.transform);
                levelBtn.GetComponent<UIComponent>().value = i.ToString();
                levelBtn.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = (i + 1).ToString();
                levelBtn.name = (i + 1).ToString();

                generatedLevelButtons.Add(levelBtn);
            }
        }
        #endregion
    }
}
