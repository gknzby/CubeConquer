using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CubeConquer.Scriptables;
using CubeConquer.Managers;

namespace CubeConquer.Components
{
    public class LevelCreator : MonoBehaviour, IInputReceiver
    {
        [SerializeField] private LevelData levelData;
        [SerializeField] private GridCellType activeCellType;
        [SerializeField] private Vector2Int newLevelDims;
        [SerializeField] private Vector3 newCellSize;
        [SerializeField] private Vector3 newCellGap;
        [SerializeField] private ColorScheme newLevelColorScheme;

        [SerializeField] private GameObject GridMainPrefab;
        private GridMain gridMain;
        private Vector2Int clickCellPos;

        Dictionary<LevelData, bool> savedLevels = new();

        private void Start()
        {
            ManagerProvider.GetManager<IInputManager>().SetDefaultReceiver(this);
            ManagerProvider.GetManager<IInputManager>().StartSendingInputs();
        }

        private void OnDestroy()
        {
            ManagerProvider.GetManager<IInputManager>()?.RemoveDefaultReceiver(this);
        }

        public void Cancel()
        {
            return;
        }

        public void Click()
        {
            OnClick();
        }
        public void Drag(Vector2 dragVec)
        {
            return;
        }
        public void Release()
        {
            OnRelease();
        }


        private void OnClick()
        {
            Vector2Int cellPos;
            if (GetCellPos(out cellPos)
                && !gridMain.IsUnreachable(cellPos))
            {
                clickCellPos = cellPos;
            }
            else
            {
                clickCellPos = new Vector2Int(-1, -1);
                Debug.Log("None");
            }
        }

        private void OnRelease()
        {
            Vector2Int cellPos;
            if (GetCellPos(out cellPos)
                && !gridMain.IsUnreachable(cellPos)
                && clickCellPos == cellPos)
            {
                gridMain.ApplyColor(cellPos, activeCellType);
            }
        }

        private bool GetCellPos(out Vector2Int cellPos)
        {
            Ray screenRay = GetScreenRay();
            RaycastHit hit;

            if (Physics.Raycast(screenRay, out hit) && hit.transform.TryGetComponent<GridMain>(out gridMain))
            {
                cellPos = gridMain.WorldToCellPos(hit.point);

                return true;
            }

            cellPos = Vector2Int.zero;
            return false;
        }

        private Ray GetScreenRay()
        {
            Camera mainCam = Camera.main;
            Vector2 mousePos = Input.mousePosition;

            return mainCam.ScreenPointToRay(mousePos);
        }

        private GridMain GetNewGrid()
        {
            if (gridMain != null)
            {
                GameObject.Destroy(gridMain.gameObject);
            }

            return GameObject.Instantiate(GridMainPrefab).GetComponent<GridMain>();
        }

        public void CreateNewLevel()
        {
            gridMain = GetNewGrid();

            gridMain.SetGridDimensions(newLevelDims);
            gridMain.SealBorders();
            gridMain.SetCellSize(newCellSize, newCellGap);
            gridMain.SetMaterialDict(newLevelColorScheme.GetMaterialDict());
            gridMain.GenerateGrid();
        }

        public void SetActiveCellType(GridCellType cellType)
        {
            activeCellType = cellType;
        }

        public void SetActiveCellType(int cellTypeInt)
        {
            SetActiveCellType((GridCellType)cellTypeInt);
        }

        public void SaveLevel()
        {
            levelData.GridDimensions = gridMain.GetGridDimensions();
            gridMain.GetCellSize(out levelData.CellSize, out levelData.CellGap);
            levelData.cellTypeArray = gridMain.GetCellTypeArray();

            UnityEditor.EditorUtility.SetDirty(levelData);
            
            //savedLevels.TryAdd(levelData, true);
        }

        public void LoadLevel()
        {
            Debug.Assert(levelData != null, "Level Data is null!");

            gridMain = GetNewGrid();

            gridMain.SetGridDimensions(levelData.GridDimensions);
            gridMain.SetCellSize(levelData.CellSize, levelData.CellGap);
            gridMain.SetMaterialDict(levelData.GetMaterialDict());
            gridMain.SetCellTypeArray(levelData.cellTypeArray);
            gridMain.GenerateGrid();
        }
    }
}
