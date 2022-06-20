using CubeConquer.Components;
using CubeConquer.Scriptables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeConquer.Managers
{
    public class LevelManager : MonoBehaviour, ILevelManager
    {
        [SerializeField] private List<LevelData> LevelList;
        [SerializeField] private Transform GridParentTransform;

        [SerializeField] private GameObject GridPrefab;
        private GridMain gridMain;

        public int LevelCount { get { return LevelList.Count; } }

        public bool LoadLevel(int index)
        {
            if(index < 0 || index >= LevelList.Count)
            {
                Debug.Log(index + " level cannot be loaded.");
                return false;
            }

            gridMain = GetNewGrid();
            LevelData levelData = LevelList[index];

            gridMain.SetGridDimensions(levelData.GridDimensions);
            gridMain.SetCellSize(levelData.CellSize, levelData.CellGap);
            gridMain.SetMaterialDict(levelData.GetMaterialDict());
            gridMain.SetCellTypeArray(levelData.cellTypeArray);
            gridMain.GenerateGrid();

            return true;
        }

        private GridMain GetNewGrid()
        {
            if(gridMain != null)
            {
                GameObject.Destroy(gridMain.gameObject);
            }
            while (GridParentTransform.childCount > 0)
            {
                GameObject.Destroy(GridParentTransform.GetChild(0).gameObject);
            }

            return GameObject.Instantiate(GridPrefab, GridParentTransform).GetComponent<GridMain>();
        }

        #region Unity Functions => Awake, OnDestroy
        private void Awake()
        {
            ManagerProvider.AddManager<ILevelManager>(this);
        }

        private void OnDestroy()
        {
            ManagerProvider.RemoveManager<ILevelManager>();
        }
        #endregion
    }
}
