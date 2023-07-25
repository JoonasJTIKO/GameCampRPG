using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace GameCampRPG.UI
{
    public class InventoryContextMenu : MenuCanvas
    {
        [SerializeField]
        private GameObject menuItem;

        private PlayerInputs inputs;

        private InputAction menuUp;

        private InputAction menuDown;

        private InputAction select;

        private List<GameObject> menuItems = new();

        private int selectedItem = 0;

        private void Awake()
        {
            if (GameInstance.Instance == null) return;

            inputs = GameInstance.Instance.GetPlayerInfo().PlayerInputs;
            menuUp = inputs.UI.MenuUp;
            menuDown = inputs.UI.MenuDown;
            select = inputs.UI.Select;
        }

        private void OnEnable()
        {
            if (inputs == null) return;

            selectedItem = 0;
            SetInputs();
            inputs.UI.Enable();
        }

        private void OnDisable()
        {
            if (inputs == null) return;

            RemoveInputs();
            inputs.UI.Disable();
        }

        public void InitializeMenu(Item item)
        {
            if (item.Equippable)
            {

            }
            else
            {
                DrawMenu(2, new string[] { "Use" });
            }
        }

        private void MoveInMenusUp(InputAction.CallbackContext callback)
        {
            if (selectedItem > 0)
            {
                selectedItem--;
            }
            else
            {
                selectedItem = menuItems.Count - 1;
            }
            UpdateSelectedItem();
        }

        private void MoveInMenusDown(InputAction.CallbackContext callback)
        {
            if (selectedItem < menuItems.Count - 1)
            {
                selectedItem++;
            }
            else
            {
                selectedItem = 0;
            }
            UpdateSelectedItem();
        }

        private void SelectPerformed(InputAction.CallbackContext callback)
        {
            Debug.Log("Select performed");
        }

        private void UpdateSelectedItem()
        {
            for (int i = 0; i < menuItems.Count; i++)
            {
                if (i == selectedItem)
                {
                    menuItems[i].transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    menuItems[i].transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }

        private void DrawMenu(int count, string[] menuItemNames)
        {
            if (menuItems.Count != 0)
            {
                foreach (GameObject menuItem in menuItems)
                {
                    Destroy(menuItem);
                }

                menuItems = new List<GameObject>();
            }

            Vector3 optionPosition = menuItem.GetComponent<RectTransform>().anchoredPosition3D;
            Vector3 newPosition;

            for (int i = 0; i < count; i++)
            {
                newPosition = new(optionPosition.x, optionPosition.y - (250 * i), optionPosition.z);
                if (i == count - 1)
                {
                    GameObject option = GameObject.Instantiate(menuItem, this.gameObject.transform);
                    menuItems.Add(option);
                    menuItems[i].GetComponent<RectTransform>().anchoredPosition3D = newPosition;
                    menuItems[i].GetComponent<TextMeshProUGUI>().text = "Cancel";
                }
                else
                {
                    GameObject option = GameObject.Instantiate(menuItem, this.gameObject.transform);
                    menuItems.Add(option);
                    menuItems[i].GetComponent<RectTransform>().anchoredPosition3D = newPosition;
                    menuItems[i].GetComponent<TextMeshProUGUI>().text = menuItemNames[i];
                }
            }
        }

        public void SetInputs()
        {
            menuUp.performed += MoveInMenusUp;
            menuDown.performed += MoveInMenusDown;
            select.performed += SelectPerformed;
            UpdateSelectedItem();
        }

        public void RemoveInputs()
        {
            menuUp.performed -= MoveInMenusUp;
            menuDown.performed -= MoveInMenusDown;
            select.performed -= SelectPerformed;
        }
    }
}
