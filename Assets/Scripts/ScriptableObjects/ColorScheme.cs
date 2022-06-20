using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CubeConquer.Components;

namespace CubeConquer.Scriptables
{
    [CreateAssetMenu(fileName = "Color Scheme", menuName = "Cube Conquer/Color Scheme", order = 2)]
    public class ColorScheme : ScriptableObject
    {
        [SerializeField] private List<CellColor> ColorList = new List<CellColor>();

        [System.Serializable]
        public class CellColor
        {
            public Color color;
            public GridCellType cellType;
            public Material cellMaterial;
        }

        public Dictionary<GridCellType, Material> GetMaterialDict()
        {
            Dictionary<GridCellType, Material> cellMaterialDict = new();
            
            foreach (CellColor cellColor in ColorList)
            {
                cellColor.cellMaterial.color = cellColor.color;
                cellMaterialDict.TryAdd(cellColor.cellType, cellColor.cellMaterial);
            }

            return cellMaterialDict;
        }
    }
}

