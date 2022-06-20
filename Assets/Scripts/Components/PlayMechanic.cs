using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CubeConquer.Managers;
using System;

namespace CubeConquer.Components
{
    public class PlayMechanic : MonoBehaviour, IInputReceiver
    {
        [SerializeField] private int placeCount = 1;

        private Queue<Vector2Int> openQ = new Queue<Vector2Int>();
        private GridMain gridMain = null;

        private Vector2Int clickCellPos;

        private void Start()
        {
            ManagerProvider.GetManager<IInputManager>().SetDefaultReceiver(this);
        }

        private void OnDestroy()
        {
            ManagerProvider.GetManager<IInputManager>()?.RemoveDefaultReceiver(this);
        }

        public void Cancel()
        {
            //throw new System.NotImplementedException();
        }

        public void Click()
        {
            OnClick();
        }

        public void Drag(Vector2 dragVec)
        {
            //throw new System.NotImplementedException();
        }

        public void Release()
        {
            OnRelease();
            if(placeCount == 0)
            {
                StartCoroutine(SpreadColors());
            }
        }

        private IEnumerator SpreadColors()
        {
            yield return null;

            List<Vector2Int> cellPosList = gridMain.GetColoredChilds();
            foreach (Vector2Int cellPos in cellPosList)
            {
                openQ.Enqueue(cellPos);
            }

            Coroutine cr = StartCoroutine(ChangeColors(new List<Vector2Int>()));

            while (openQ.Count > 0)
            {
                Debug.Log(openQ.Count);
                yield return cr;
                cellPosList = new List<Vector2Int>();
                Queue<Vector2Int> tempQuery = new Queue<Vector2Int>(openQ);
                openQ.Clear();
                foreach (Vector2Int cellPos in tempQuery)
                {
                    SpreadAround(ref cellPosList, cellPos);
                }
                cr = StartCoroutine(ChangeColors(cellPosList));
            }
        }

        private void SpreadAround(ref List<Vector2Int> cellPosList, Vector2Int cellPos)
        {
            SpreadAround(ref cellPosList, cellPos.x, cellPos.y);
        }

        private void SpreadAround(ref List<Vector2Int> cellPosList, int x, int y)
        {
            GridCellType gridCellType = gridMain.GetGridCellType(x, y);

            SpreadChild(ref cellPosList, x - 1, y, gridCellType);
            SpreadChild(ref cellPosList, x, y - 1, gridCellType);
            SpreadChild(ref cellPosList, x + 1, y, gridCellType);
            SpreadChild(ref cellPosList, x, y + 1, gridCellType);
        }

        private void SpreadChild(ref List<Vector2Int> cellPosList, int x, int y, GridCellType gridCellType)
        {
            if (gridMain.IsPlaceable(x, y))
            {
                gridMain.ChangeType(x, y, gridCellType);
                cellPosList.Add(new Vector2Int(x, y));
                openQ.Enqueue(new Vector2Int(x, y));
            }
        }

        private IEnumerator ChangeColors(List<Vector2Int> cellPosList)
        {
            foreach(Vector2Int cellPos in cellPosList)
            {
                gridMain.ApplyColor(cellPos);
            }
            //yield return null;
            yield return new WaitForSeconds(0.2f);
        }

        private void OnClick()
        {
            Vector2Int cellPos;
            if(GetCellPos(out cellPos) 
                && gridMain.IsPlaceable(cellPos))
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
                && gridMain.IsPlaceable(cellPos)
                && clickCellPos == cellPos)
            {
                placeCount--;
                gridMain.ApplyColor(cellPos, GridCellType.PlayerColor);
                openQ.Enqueue(cellPos);
            }
        }

        private Ray GetScreenRay()
        {
            Camera mainCam = Camera.main;
            Vector2 mousePos = Input.mousePosition;

            return mainCam.ScreenPointToRay(mousePos);
        }

        private bool GetCellPos(out Vector2Int cellPos)
        {
            Ray screenRay = GetScreenRay();
            RaycastHit hit;

            if(Physics.Raycast(screenRay, out hit) && hit.transform.TryGetComponent<GridMain>(out gridMain))
            {
                cellPos = gridMain.WorldToCellPos(hit.point);

                return true;
            }

            cellPos = Vector2Int.zero;
            return false;
        }
    }
}
