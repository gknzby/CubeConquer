using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeConquer.Components
{
    [System.Serializable]
    public enum GridCellType
    {
        Unreachable = -2,
        Wall = -1,
        Blank = 0,
        PlayerColor = 1,
        ColorA = 2,
        ColorB = 3,
        ColorC = 4
    }
    public class GridCell : MonoBehaviour
    {
        [SerializeField] private GameObject TargetObject;

        public void ChangeColor(Material colorMaterial)
        {
            TargetObject.GetComponent<Renderer>().material = colorMaterial;
        }
    }
}
