using System.Collections.Generic;
using UnityEngine;
using CubeConquer.Components;

namespace CubeConquer.Scriptables
{
    [CreateAssetMenu(fileName = "Level", menuName = "Cube Conquer/Level", order = 1)]
    public class LevelData : ScriptableObject
    {
        [SerializeField] public ColorScheme colorScheme;

        [SerializeField] public string LevelName;

        //[HideInInspector]
        [SerializeField] public Vector2Int GridDimensions;

        //[HideInInspector]
        [SerializeField] public Vector3 CellSize;

        //[HideInInspector]
        [SerializeField] public Vector3 CellGap;

        //[HideInInspector]
        [SerializeField] public GridCellType[] cellTypeArray;

        //[HideInInspector]
        [SerializeField] public bool FirstInit = true;

        [SerializeField] public int PlaceableCount = 1;

        public Dictionary<GridCellType, Material> GetMaterialDict()
        {
            return colorScheme.GetMaterialDict();
        }

        //public void SetGridDimensions(Vector2Int dimensions)
        //{
        //    GridCellType[,] newTypeArray = new GridCellType[dimensions.x, dimensions.y];
        //    BlankGrid(ref newTypeArray, dimensions);

        //    if(!firstInit)
        //    {
        //        OldArrayToNewArray(ref newTypeArray, dimensions);
        //    }

        //    cellTypeArray = newTypeArray;
        //    gridDimensions = dimensions;


        //    firstInit = false;
        //}

        //public void CheckUpdate()
        //{
        //    if(firstInit)
        //    {
        //        UpdateGridDimensions();
        //    }
        //}

        //public void UpdateGridDimensions()
        //{
        //    SetGridDimensions(GridDimensions);
        //}

        //private void BlankGrid(ref GridCellType[,] newArray, Vector2Int newDimensions)
        //{
        //    for(int i = 0; i < newDimensions.x; i++)
        //    {
        //        for(int j = 0; j < newDimensions.y; j++)
        //        {
        //            newArray[i, j] = GridCellType.Blank;
        //        }
        //    }

        //    SealBorders(ref newArray, newDimensions);
        //}

        //private void OldArrayToNewArray(ref GridCellType[,] newArray, Vector2Int newDimensions)
        //{
        //    UnSealBorders(ref this.cellTypeArray, gridDimensions);

        //    int x = newDimensions.x > gridDimensions.x ? gridDimensions.x : newDimensions.x;
        //    int y = newDimensions.y > gridDimensions.y ? gridDimensions.y : newDimensions.y;

        //    for(int i = 0; i < x; i++)
        //    {
        //        for(int j = 0; j < y; j++)
        //        {
        //            newArray[i, j] = cellTypeArray[i, j];
        //        }
        //    }

        //    SealBorders(ref newArray, newDimensions);
        //}

        //private void SealBorders(ref GridCellType[,] cellTypeArray, Vector2Int dimensions)
        //{
        //    for (int i = 0; i < dimensions.x; i++)
        //    {
        //        cellTypeArray[i, 0] = GridCellType.Unreachable;
        //        cellTypeArray[i, dimensions.y - 1] = GridCellType.Unreachable;
        //    }
        //    for (int j = 0; j < dimensions.y; j++)
        //    {
        //        cellTypeArray[0, j] = GridCellType.Unreachable;
        //        cellTypeArray[dimensions.x - 1, j] = GridCellType.Unreachable;
        //    }
        //}

        //private void UnSealBorders(ref GridCellType[,] cellTypeArray, Vector2Int dimensions)
        //{
        //    for (int i = 0; i < dimensions.x; i++)
        //    {
        //        cellTypeArray[i, 0] = GridCellType.Blank;
        //        cellTypeArray[i, dimensions.y - 1] = GridCellType.Blank;
        //    }
        //    for (int j = 0; j < dimensions.y; j++)
        //    {
        //        cellTypeArray[0, j] = GridCellType.Blank;
        //        cellTypeArray[dimensions.x - 1, j] = GridCellType.Blank;
        //    }
        //}

    }
}
