using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeConquer.Components
{
    public class GridMain : MonoBehaviour
    {
        [SerializeField] private GameObject GridChildPrefab;

        private GridChild[,] elements;
        private GridChildType[,] elementsInt;
        private int width, height;

        private Vector3 cellSize = Vector3.one;
        private Vector3 cellGap = new Vector3(0.5f, 0.1f, 0.2f);

        [SerializeField] private Material UnreachableMat;
        [SerializeField] private Material WallMat;
        [SerializeField] private Material BlankMat;
        [SerializeField] private Material ColorAMat;
        [SerializeField] private Material ColorBMat;
        [SerializeField] private Material ColorCMat;
        [SerializeField] private Material ColorDMat;

        private Dictionary<GridChildType, Material> typeColor;

        private void Awake()
        {
            typeColor = new Dictionary<GridChildType, Material>();
            typeColor.Add(GridChildType.Unreachable, UnreachableMat);
            typeColor.Add(GridChildType.Wall, WallMat);
            typeColor.Add(GridChildType.Blank, BlankMat);
            typeColor.Add(GridChildType.ColorA, ColorAMat);
            typeColor.Add(GridChildType.ColorB, ColorBMat);
            typeColor.Add(GridChildType.ColorC, ColorCMat);
            typeColor.Add(GridChildType.ColorD, ColorDMat);
        }

        private void Start()
        {
            SetGridSize(10, 10);
            elementsInt[3, 5] = GridChildType.ColorB;
            elementsInt[7, 2] = GridChildType.ColorC;
            elementsInt[5, 5] = GridChildType.ColorD;
            GenerateGrid();
        }

        public void SetGridSize(int width, int height)
        {
            this.width = width;
            this.height = height;

            elements = new GridChild[width, height];
            elementsInt = new GridChildType[width, height];

            TEMP_TYPESET();
        }

        private void TEMP_TYPESET()
        {
            for(int i = 0; i < width; i++)
            {
                elementsInt[0, i] = GridChildType.Unreachable;
                elementsInt[i, 0] = GridChildType.Unreachable;
                elementsInt[width - 1, i] = GridChildType.Unreachable;
                elementsInt[i, height - 1] = GridChildType.Unreachable;
            }
        }


        public void GenerateGrid()
        {
            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    CreateElement(out elements[i, j], i, j);
                    PositionElement(elements[i, j], i, j);
                }
            }

            BoxCollider bc = this.GetComponent<BoxCollider>();
            Vector3 bcSize = GetBCSize();

            bc.size = bcSize;
            bc.center = (bcSize - cellSize )/ 2f ;
        }

        private Vector3 GetBCSize()
        {
            Vector3 bcSize = Vector3.one;

            bcSize.x = cellSize.x * width + cellGap.x * (width - 1);
            bcSize.y = cellSize.y * height + cellGap.y * (height - 1);

            return bcSize;
        }

        private void CreateElement(out GridChild gridChild, int x, int y)
        {
            gridChild = GameObject.Instantiate(GridChildPrefab, this.transform).GetComponent<GridChild>();
            gridChild.SetGridChild(elementsInt[x, y], x, y, typeColor[elementsInt[x,y]]);
        }

        private void PositionElement(GridChild gridChild, int x, int y)
        {
            Vector3 origin = this.transform.position;
            origin.x += cellSize.x * x + cellGap.x * x;
            origin.y += cellSize.y * y + cellGap.y * y;

            gridChild.transform.position = origin;
        }

        public void ChangeType(int x, int y, GridChildType gt)
        {
            elementsInt[x, y] = gt;
            elements[x,y].gridChildType = gt;
        }

        public void PlaceColor(int x, int y)
        {
            elements[x, y].ChangeColor(typeColor[GridChildType.ColorA]);
        }

        public void ApplyColor(int x, int y)
        {
            elements[x, y].ChangeColor(typeColor[elementsInt[x,y]]);
        }

        public bool IsPlaceable(int x, int y)
        {
            return elements[x, y].gridChildType == GridChildType.Blank;
        }

        public GridChild GetGridChild(int x, int y)
        {
            return elements[x, y];
        }

        public List<GridChild> GetColoredChilds()
        {
            List<GridChild> list = new List<GridChild>();

            foreach(GridChild child in elements)
            {
                if(((int)child.gridChildType > 0))
                {
                    list.Add(child);
                }
            }

            return list;
        }
    }
}

