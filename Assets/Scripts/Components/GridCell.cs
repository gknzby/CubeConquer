using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeConquer.Components
{
    public enum GridCellType
    {
        Unreachable = -2,
        Wall = -1,
        Blank = 0,
        ColorA = 1,
        ColorB = 2,
        ColorC = 3,
        ColorD = 4
    }
    public class GridCell : MonoBehaviour
    {
        public void ChangeColor(Material colorMaterial)
        {
            this.GetComponent<Renderer>().material = colorMaterial;
        }
    }
}
