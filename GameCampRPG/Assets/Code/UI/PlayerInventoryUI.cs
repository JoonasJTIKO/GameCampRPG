using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace GameCampRPG.UI
{
    public class PlayerInventoryUI : MenuCanvas
    {
        [SerializeField]
        private PauseMenuNavigation pauseMenuNavigation;

        [SerializeField]
        private GameObject itemFrame;

        private List<GameObject> inventoryMenuItems = new();

        private List<IItem> playerItems;

        private PlayerInputs inputs;

        private InputAction menuUp;

        private InputAction menuDown;

        private InputAction menuLeft;

        private InputAction menuRight;

        private InputAction select;

        private InputAction escape;

        private int selectedItem = 0;

        private void Awake()
        {
            if (GameInstance.Instance == null) return;

            inputs = GameInstance.Instance.GetPlayerInfo().PlayerInputs;
            menuUp = inputs.UI.MenuUp;
            menuDown = inputs.UI.MenuDown;
            menuLeft = inputs.UI.MenuLeft;
            menuRight = inputs.UI.MenuRight;
            select = inputs.UI.Select;
            escape = inputs.UI.Escape;
        }

        private void OnEnable()
        {
            for (int i = 0; i < 25; i++)
            {
                GameObject menuFrame = GameObject.Instantiate(itemFrame, this.transform);
                inventoryMenuItems.Add(menuFrame);
            }
        }

        private void OnDisable()
        {
            foreach (GameObject menuListItem in inventoryMenuItems)
            {
                Destroy(menuListItem);
            }
            inventoryMenuItems.Clear();
        }

        public void EnableInputs()
        {
            if (inputs == null) return;

            selectedItem = 0;
            menuUp.performed += MoveInMenusUp;
            menuDown.performed += MoveInMenusDown;
            menuLeft.performed += MoveInMenusLeft;
            menuRight.performed += MoveInMenusRight;
            select.performed += SelectPerformed;
            escape.performed += EscapePerformed;
            UpdateSelectedItem();
        }

        public void DisableInputs()
        {
            if (inputs == null) return;

            menuUp.performed -= MoveInMenusUp;
            menuDown.performed -= MoveInMenusDown;
            menuLeft.performed -= MoveInMenusLeft;
            menuRight.performed -= MoveInMenusRight;
            select.performed -= SelectPerformed;
            escape.performed -= EscapePerformed;
        }

        private void MoveInMenusUp(InputAction.CallbackContext callback)
        {
            if (selectedItem - 5 >= 0)
            {
                selectedItem -= 5;
            }
            else
            {
                selectedItem = inventoryMenuItems.Count - (5 - selectedItem);
            }
            UpdateSelectedItem();
        }

        private void MoveInMenusDown(InputAction.CallbackContext callback)
        {
            if (selectedItem + 5 <= inventoryMenuItems.Count - 1)
            {
                selectedItem += 5;
            }
            else
            {
                selectedItem = 5 - (inventoryMenuItems.Count - selectedItem);
            }
            UpdateSelectedItem();
        }

        private void MoveInMenusLeft(InputAction.CallbackContext callback)
        {
            if (selectedItem > 0)
            {
                selectedItem--;
            }
            else
            {
                selectedItem = inventoryMenuItems.Count - 1;
            }
            UpdateSelectedItem();
        }

        private void MoveInMenusRight(InputAction.CallbackContext callback)
        {
            if (selectedItem < inventoryMenuItems.Count - 1)
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

        }

        private void EscapePerformed(InputAction.CallbackContext callback)
        {
            DisableInputs();
            pauseMenuNavigation.SetInputs();
        }

        private void UpdateSelectedItem()
        {
            for (int i = 0; i < inventoryMenuItems.Count; i++)
            {
                if (i == selectedItem)
                {
                    inventoryMenuItems[i].GetComponent<Image>().color = Color.red;
                }
                else
                {
                    inventoryMenuItems[i].GetComponent<Image>().color = Color.black;
                }
            }
        }
    }
}
