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

        private Queue<GridChild> openQ = new Queue<GridChild>();
        private GridMain gridMain = null;

        private void Start()
        {
            ManagerProvider.GetManager<IInputManager>().SetDefaultReceiver(this);
        }

        private void OnDestroy()
        {
            ManagerProvider.GetManager<IInputManager>()?.RemoveDefaultReceiver(this);
        }

        private bool isPlaceable = false;

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
            if(placeCount == 0)
            {
                StartCoroutine(SpreadColors());
            }
        }
        int counter = 0;
        private IEnumerator SpreadColors()
        {
            yield return null;

            List<GridChild> ll = gridMain.GetColoredChilds();
            foreach (GridChild gridiChildi in ll)
            {
                openQ.Enqueue(gridiChildi);
            }

            Coroutine cr = StartCoroutine(ChangeColors(new List<GridChild>()));

            while (openQ.Count > 0)
            {
                Debug.Log(counter++);
                yield return cr;
                List<GridChild> gcList = new List<GridChild>();
                Queue<GridChild> tempQ = new Queue<GridChild>(openQ);
                openQ.Clear();
                foreach (GridChild sp in tempQ)
                {
                    SpreadAround(gcList, sp);
                }
                cr = StartCoroutine(ChangeColors(gcList));
            }
        }

        private List<GridChild> SpreadAround(List<GridChild> gcList, GridChild sp)
        {
            int x = sp.x, y = sp.y;

            SpreadChild(x - 1, y, sp.gridChildType, gcList);
            SpreadChild(x, y - 1, sp.gridChildType, gcList);
            SpreadChild(x + 1, y, sp.gridChildType, gcList);
            SpreadChild(x, y + 1, sp.gridChildType, gcList);

            return gcList;
        }

        private void SpreadChild(int x, int y, GridChildType gp, List<GridChild> gcList)
        {
            if (gridMain.IsPlaceable(x, y))
            {
                gridMain.ChangeType(x, y, gp);
                gcList.Add(gridMain.GetGridChild(x, y));
                openQ.Enqueue(gridMain.GetGridChild(x, y));
            }
        }

        private IEnumerator ChangeColors(List<GridChild> grs)
        {
            foreach(GridChild kvp in grs)
            {
                gridMain.ApplyColor(kvp.x, kvp.y);
            }
            yield return new WaitForSeconds(1f);
        }

        private void OnClick()
        {
            GridChild gridChild;
            if(GetGetGet(out gridChild, out gridMain) && gridMain.IsPlaceable(gridChild.x, gridChild.y))
            {
                gridMain.ChangeType(gridChild.x, gridChild.y, GridChildType.ColorA);
                gridMain.PlaceColor(gridChild.x, gridChild.y);
                placeCount--;
                openQ.Enqueue(gridChild);
            }
            else
            {
                Debug.Log("None");
            }
        }


        private Ray GetScreenRay()
        {
            Camera mainCam = Camera.main;
            Vector2 mousePos = Input.mousePosition;

            return mainCam.ScreenPointToRay(mousePos);
        }

        private bool GetGetGet(out GridChild gc, out GridMain gm)
        {
            Ray screenRay = GetScreenRay();
            RaycastHit[] hits = null;

            gc = null;
            gm = null;

            if((hits = Physics.RaycastAll(screenRay)) != null && hits.Length > 1)
            {
                foreach (RaycastHit hit in hits)
                {
                    gc = (gc == null) ? ((hit.transform.GetComponent<GridChild>() != null) ? hit.transform.GetComponent<GridChild>() : gc) : gc;
                    gm = (gm == null) ? ((hit.transform.GetComponent<GridMain>() != null) ? hit.transform.GetComponent<GridMain>() : gm) : gm;
                }
            }

            return (gc != null && gm != null) ? true : false;
        }

        //private bool GetGridChild(out GridChild gridChild)
        //{
        //    Ray screenRay = GetScreenRay();
        //    RaycastHit hit;
        //    LayerMask gridChildLayer = LayerMask.NameToLayer("GridChild");

        //    if (Physics.Raycast(screenRay, out hit, maxDistance: 1000f, layerMask: ~gridChildLayer)
        //        && hit.transform.TryGetComponent<GridChild>(out gridChild))
        //    {
        //        return true;
        //    }

        //    gridChild = null;
        //    return false;
        //}

        //private bool GetGridMain(out GridMain gridMain)
        //{
        //    Ray screenRay = GetScreenRay();
        //    RaycastHit hit;
        //    LayerMask gridMainLayer = LayerMask.NameToLayer("GridMain");

        //    if (Physics.Raycast(screenRay, out hit, maxDistance : 1000f, layerMask : ~gridMainLayer)
        //        && hit.transform.TryGetComponent<GridMain>(out gridMain))
        //    {
        //        return true;
        //    }

        //    gridMain = null;
        //    return false;
        //}
    }
}
