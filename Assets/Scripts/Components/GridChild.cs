using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeConquer.Components
{
    public enum GridChildType
    {
        Unreachable = -2,
        Wall = -1,
        Blank = 0,
        ColorA = 1,
        ColorB = 2,
        ColorC = 3,
        ColorD = 4
    }
    public class GridChild : MonoBehaviour
    {
        public bool isSpreadable = true;
        public GridChildType gridChildType;
        public int x, y;
        public Color gridColor;

        public void SetGridChild(GridChildType gridChildType, int x, int y, Material mat)
        {
            this.x = x;
            this.y = y;

            this.gridChildType = gridChildType;
            this.GetComponent<Renderer>().material = mat;
        }

        public void ChangeColor(Material mat)
        {
            this.GetComponent<Renderer>().material = mat;
        }
    }
}
