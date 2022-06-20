using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CubeConquer.Managers;

namespace CubeConquer.Components
{
    public class UIMenu : MonoBehaviour, IUIMenu
    {

        #region IUIMenu
        public virtual void ShowMenu()
        {
            this.MenuObj.SetActive(true);
        }
        public virtual void HideMenu()
        {
            this.MenuObj.SetActive(false);
        }

        public virtual string GetMenuName()
        {
            return this.MenuName;
        }
        #endregion

        [SerializeField] protected GameObject MenuObj;
        [SerializeField] protected string MenuName;

        private void Start()
        {
            RegisterToUIManager();
        }

        public void RegisterToUIManager()
        {
            if (MenuName.Length < 1)
            {
                MenuName = this.gameObject.name; //I know, I can write 'name' or 'this.name' but this is eaiser to understand when read
            }

            ManagerProvider.GetManager<IUIManager>().RegisterMenu(this);
        }

    }
}
