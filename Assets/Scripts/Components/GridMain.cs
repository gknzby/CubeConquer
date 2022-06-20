using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeConquer.Components
{
    public class GridMain : MonoBehaviour
    {
        [SerializeField] private GameObject GridChildPrefab;

        private Vector2Int gridDimensions; //Required
        private Vector3 gridOrigin;

        private GridCell[,] cellArray;
        private GridCellType[,] cellTypeArray; //Required

        private Vector3 gridSize;
        private Vector3 cellSize; //Required
        private Vector3 cellGap; //Required

        private Dictionary<GridCellType, Material> cellTypeMaterials; //Required

        //#region Test Variables
        //[SerializeField] private Vector2Int TempDims;

        //[SerializeField] private Material UnreachableMat;
        //[SerializeField] private Material WallMat;
        //[SerializeField] private Material BlankMat;
        //[SerializeField] private Material ColorAMat;
        //[SerializeField] private Material ColorBMat;
        //[SerializeField] private Material ColorCMat;
        //[SerializeField] private Material ColorDMat;

        //[SerializeField] private Vector3 TempCellSize = Vector3.one;
        //[SerializeField] private Vector3 TempCellGap = new Vector3(0.1f, 0.1f, 0.1f);
        //#endregion

        //#region TEST FUNCS
        //private void Awake()
        //{
        //    //TestSetup();
        //}
        //private void TestSetup()
        //{
        //    SetMatDict();
        //    SetGridValues();
        //    GenerateGrid();
        //}
        //private void SetGridValues()
        //{
        //    SetGridDimensions(this.TempDims);
        //    SetCellSize(this.TempCellSize, this.TempCellGap);
        //    CalculateGridOrigin();
        //    SealBorders();
        //    SetEnemies();
        //}
        //private void SetMatDict()
        //{
        //    cellTypeMaterials = new Dictionary<GridCellType, Material>();
        //    cellTypeMaterials.Add(GridCellType.Unreachable, UnreachableMat);
        //    cellTypeMaterials.Add(GridCellType.Wall, WallMat);
        //    cellTypeMaterials.Add(GridCellType.Blank, BlankMat);
        //    cellTypeMaterials.Add(GridCellType.ColorA, ColorAMat);
        //    cellTypeMaterials.Add(GridCellType.ColorB, ColorBMat);
        //    cellTypeMaterials.Add(GridCellType.ColorC, ColorCMat);
        //    cellTypeMaterials.Add(GridCellType.ColorD, ColorDMat);
        //}


        //private void SetEnemies()
        //{
        //    cellTypeList[3][5] = GridCellType.ColorB;
        //    cellTypeList[7][2] = GridCellType.ColorC;
        //    cellTypeList[5][5] = GridCellType.ColorD;
        //}
        //#endregion

        #region Passed Funcs

        private void CalculateGridOrigin()
        {
            gridSize = new Vector3(cellSize.x * gridDimensions.x + cellGap.x * (gridDimensions.x - 1),
                                    cellSize.y * gridDimensions.y + cellGap.y * (gridDimensions.y - 1),
                                    0f);
            gridOrigin = transform.position - gridSize / 2f;

            gridSize.z = cellSize.z;
        }

        public void SetGridDimensions(int width, int height)
        {
            gridDimensions.x = width;
            gridDimensions.y = height;

            cellArray = new GridCell[width, height];

            cellTypeArray = new GridCellType[width, height];
        }

        public Vector2Int GetGridDimensions()
        {
            return gridDimensions;
        }

        public void SetGridDimensions(Vector2Int gridDims)
        {
            SetGridDimensions(gridDims.x, gridDims.y);
        }

        public void SetCellTypeArray(GridCellType[] cellTypeArr)
        {
            for(int i = 0; i < gridDimensions.x * gridDimensions.y; i++)
            {
                cellTypeArray[i/gridDimensions.y, i%gridDimensions.y] = cellTypeArr[i];
            }
        }

        public GridCellType[] GetCellTypeArray()
        {
            GridCellType[] cellTypeArr = new GridCellType[gridDimensions.x * gridDimensions.y];
            for(int i = 0; i < cellTypeArr.Length; i++)
            {
                cellTypeArr[i] = cellTypeArray[i / gridDimensions.y, i % gridDimensions.y];
            }

            return cellTypeArr;
        }

        public void SetCellSize(Vector3 cellSize, Vector3 cellGap)
        {
            this.cellSize = cellSize;
            this.cellGap = cellGap;
        }

        public void GetCellSize(out Vector3 cellSize, out Vector3 cellGap)
        {
            cellSize = this.cellSize;
            cellGap = this.cellGap;
        }

        public void SetMaterialDict(Dictionary<GridCellType, Material> materialDict)
        {
            this.cellTypeMaterials = materialDict;
        }

        public void SealBorders()
        {
            for (int i = 0; i < gridDimensions.x; i++)
            {
                cellTypeArray[i, 0] = GridCellType.Unreachable;
                cellTypeArray[i, gridDimensions.y - 1] = GridCellType.Unreachable;
            }
            for (int j = 0; j < gridDimensions.y; j++)
            {
                cellTypeArray[0, j] = GridCellType.Unreachable;
                cellTypeArray[gridDimensions.x - 1, j] = GridCellType.Unreachable;
            }
        }
        #endregion


        public void GenerateGrid()
        {
            CalculateGridOrigin();

            for(int i = 0; i < gridDimensions.x; i++)
            {
                for(int j = 0; j < gridDimensions.y; j++)
                {
                    CreateElement(out cellArray[i, j], i, j);
                    PositionElement(cellArray[i, j], i, j);
                }
            }
                        
            CalculateColliderDimensions();
        }

        private void CalculateColliderDimensions()
        {
            BoxCollider boxCollider = this.GetComponent<BoxCollider>();

            boxCollider.size = gridSize;
        }

        private void CreateElement(out GridCell gridChild, int x, int y)
        {
            gridChild = GameObject.Instantiate(GridChildPrefab, this.transform).GetComponent<GridCell>();
            gridChild.ChangeColor(cellTypeMaterials[cellTypeArray[x, y]]);
        }

        private void PositionElement(GridCell gridChild, int x, int y)
        {
            Vector3 childPos = gridOrigin;
            childPos.x += cellSize.x * x + cellGap.x * x;
            childPos.y += cellSize.y * y + cellGap.y * y;

            gridChild.transform.position = childPos;
        }

        public void ChangeType(int x, int y, GridCellType cellType)
        {
            cellTypeArray[x, y] = cellType;
        }

        public bool IsUnreachable(Vector2Int cellPos)
        {
            return IsUnreachable(cellPos.x, cellPos.y);
        }

        public bool IsUnreachable(int x, int y)
        {
            return cellTypeArray[x, y] == GridCellType.Unreachable;
        }

        public bool IsPlaceable(Vector2Int cellPos)
        {
            return IsPlaceable(cellPos.x, cellPos.y);
        }

        public bool IsPlaceable(int x, int y)
        {
            return cellTypeArray[x, y] == GridCellType.Blank;
        }

        public List<Vector2Int> GetColoredChilds()
        {
            List<Vector2Int> cellPosList = new List<Vector2Int>();

            for(int i = 0; i < gridDimensions.x; i++)
            {
                for(int j = 0; j < gridDimensions.y; j++)
                {
                    if((int)cellTypeArray[i, j] > 0)
                    {
                        cellPosList.Add(new Vector2Int(i, j));
                    }
                }
            }

            return cellPosList;
        }

        public Vector2Int WorldToCellPos(Vector3 worldPos)
        {
            Vector2Int cellPos = Vector2Int.zero;

            Vector3 relativePos = worldPos - gridOrigin;

            cellPos.x = Mathf.FloorToInt(relativePos.x/(cellSize.x + cellGap.x));
            cellPos.y = Mathf.FloorToInt(relativePos.y/(cellSize.y + cellGap.y));

            return cellPos;
        }
        public GridCellType GetGridCellType(int x, int y)
        {
            return cellTypeArray[x, y];
        }

        public void ApplyColor(Vector2Int cellPos)
        {
            cellArray[cellPos.x, cellPos.y].ChangeColor(cellTypeMaterials[cellTypeArray[cellPos.x, cellPos.y]]);
        }
        public void ApplyColor(Vector2Int cellPos, GridCellType cellType)
        {
            cellTypeArray[cellPos.x, cellPos.y] = cellType;
            ApplyColor(cellPos);
        }
    }
}

