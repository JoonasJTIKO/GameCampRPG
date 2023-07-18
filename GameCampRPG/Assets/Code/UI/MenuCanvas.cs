using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameCampRPG.UI
{
    public class MenuCanvas : MonoBehaviour
    {
        [SerializeField]
        protected EventSystem eventSystem;

        [SerializeField]
        protected GameObject initialSelectedObject;

        public virtual void Show()
        {
            gameObject.SetActive(true);

            if (initialSelectedObject != null)
            {
                eventSystem.SetSelectedGameObject(initialSelectedObject);
            }
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
