using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeConquer.Components
{
    public class GridMain : MonoBehaviour
    {
        #region Passed Variables
        [SerializeField] private GameObject GridChildPrefab;

        private Vector2Int gridDimensions;
        private Vector3 gridOrigin;
        #endregion

        #region Test Variables
        [SerializeField] private Vector2Int TempDims;

        [SerializeField] private Material UnreachableMat;
        [SerializeField] private Material WallMat;
        [SerializeField] private Material BlankMat;
        [SerializeField] private Material ColorAMat;
        [SerializeField] private Material ColorBMat;
        [SerializeField] private Material ColorCMat;
        [SerializeField] private Material ColorDMat;
        private Dictionary<GridCellType, Material> typeColor;

        [SerializeField] private Vector3 TempCellSize = Vector3.one;
        [SerializeField] private Vector3 TempCellGap = new Vector3(0.1f, 0.1f, 0.1f);
        #endregion

        #region Beklemede
        private GridCell[,] gridCells;
        private GridCellType[,] gridCellsType;

        private Vector3 cellSize;
        private Vector3 cellGap;
        #endregion

        #region TEST FUNCS
        private void Awake()
        {
            TestSetup();
        }
        private void TestSetup()
        {
            SetMatDict();
            SetGridValues();
            GenerateGrid();
        }
        private void SetGridValues()
        {
            SetGridSize(this.TempDims);
            SetCellSize(this.TempCellSize, this.TempCellGap);
            gridOrigin = transform.position + cellSize / 2f;
            SealBorders();
            SetEnemies();
        }
        private void SetMatDict()
        {
            typeColor = new Dictionary<GridCellType, Material>();
            typeColor.Add(GridCellType.Unreachable, UnreachableMat);
            typeColor.Add(GridCellType.Wall, WallMat);
            typeColor.Add(GridCellType.Blank, BlankMat);
            typeColor.Add(GridCellType.ColorA, ColorAMat);
            typeColor.Add(GridCellType.ColorB, ColorBMat);
            typeColor.Add(GridCellType.ColorC, ColorCMat);
            typeColor.Add(GridCellType.ColorD, ColorDMat);
        }

        private void SetEnemies()
        {
            gridCellsType[3, 5] = GridCellType.ColorB;
            gridCellsType[7, 2] = GridCellType.ColorC;
            gridCellsType[5, 5] = GridCellType.ColorD;
        }
        #endregion

        #region Passed Funcs
        public void SetGridSize(int width, int height)
        {
            gridDimensions.x = width;
            gridDimensions.y = height;

            gridCells = new GridCell[width, height];
            gridCellsType = new GridCellType[width, height];
        }

        public void SetGridSize(Vector2Int gridDims)
        {
            SetGridSize(gridDims.x, gridDims.y);
        }

        public void SetCellSize(Vector3 cellSize, Vector3 cellGap)
        {
            this.cellSize = cellSize;
            this.cellGap = cellGap;
        }

        private void SealBorders()
        {
            for (int i = 0; i < gridDimensions.x; i++)
            {
                gridCellsType[i, 0] = GridCellType.Unreachable;
                gridCellsType[i, gridDimensions.y - 1] = GridCellType.Unreachable;
            }
            for (int j = 0; j < gridDimensions.y; j++)
            {
                gridCellsType[0, j] = GridCellType.Unreachable;
                gridCellsType[gridDimensions.x - 1, j] = GridCellType.Unreachable;
            }
        }
        #endregion


        public void GenerateGrid()
        {
            for(int i = 0; i < gridDimensions.x; i++)
            {
                for(int j = 0; j < gridDimensions.y; j++)
                {
                    CreateElement(out gridCells[i, j], i, j);
                    PositionElement(gridCells[i, j], i, j);
                }
            }

            BoxCollider bc = this.GetComponent<BoxCollider>();
            Vector3 bcSize = GetBCSize();

            bc.size = bcSize;
            bc.center = gridOrigin + (bcSize - cellSize )/ 2f ;
        }

        private Vector3 GetBCSize()
        {
            Vector3 bcSize = Vector3.one;

            bcSize.x = cellSize.x * gridDimensions.x + cellGap.x * (gridDimensions.x - 1);
            bcSize.y = cellSize.y * gridDimensions.y + cellGap.y * (gridDimensions.y - 1);

            return bcSize;
        }

        private void CreateElement(out GridCell gridChild, int x, int y)
        {
            gridChild = GameObject.Instantiate(GridChildPrefab, this.transform).GetComponent<GridCell>();
            gridChild.ChangeColor(typeColor[gridCellsType[x,y]]);
        }

        private void PositionElement(GridCell gridChild, int x, int y)
        {
            Vector3 childPos = gridOrigin;
            childPos.x += cellSize.x * x + cellGap.x * x;
            childPos.y += cellSize.y * y + cellGap.y * y;

            gridChild.transform.position = childPos;
        }

        public void ChangeType(int x, int y, GridCellType gt)
        {
            gridCellsType[x, y] = gt;
        }

        public void PlaceColor(int x, int y)
        {
            gridCells[x, y].ChangeColor(typeColor[GridCellType.ColorA]);
        }

        public void ApplyColor(Vector2Int cellPos)
        {
            gridCells[cellPos.x, cellPos.y].ChangeColor(typeColor[gridCellsType[cellPos.x, cellPos.y]]);
        }

        public bool IsPlaceable(Vector2Int cellPos)
        {
            return IsPlaceable(cellPos.x, cellPos.y);
        }
        public bool IsPlaceable(int x, int y)
        {
            return gridCellsType[x, y] == GridCellType.Blank;
        }

        public GridCell GetGridChild(int x, int y)
        {
            return gridCells[x, y];
        }

        public List<Vector2Int> GetColoredChilds()
        {
            List<Vector2Int> cellPosList = new List<Vector2Int>();

            for(int i = 0; i < gridDimensions.x; i++)
            {
                for(int j = 0; j < gridDimensions.y; j++)
                {
                    if((int)gridCellsType[i, j] > 0)
                    {
                        cellPosList.Add(new Vector2Int(i, j));
                    }
                }
            }

            return cellPosList;
        }

        public Vector2Int GetXY(Vector3 hitPos)
        {
            Vector2Int newVec = Vector2Int.zero;

            Vector3 relativePos = hitPos - transform.position;

            newVec.x = Mathf.FloorToInt(relativePos.x/(cellSize.x + cellGap.x));
            newVec.y = Mathf.FloorToInt(relativePos.y/(cellSize.y + cellGap.y));

            return newVec;
        }

        public void ApplyPlayerColor(Vector2Int cellPos)
        {
            gridCellsType[cellPos.x, cellPos.y] = GridCellType.ColorA;
            ApplyColor(cellPos);
        }

        public GridCellType GetGridCellType(int x, int y)
        {
            return gridCellsType[x, y];
        }
    }
}

