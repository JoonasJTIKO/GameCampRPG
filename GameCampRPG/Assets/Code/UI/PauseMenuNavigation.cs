using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

namespace GameCampRPG.UI
{
    public class PauseMenuNavigation : MenuCanvas
    {
        [SerializeField]
        private PlayerInventoryUI playerInventoryUI;

        [SerializeField]
        private ItemInfo itemInfo;

        [SerializeField]
        private RawImage charactersUI;

        private PlayerInputs inputs;

        private InputAction menuUp;

        private InputAction menuDown;

        private InputAction select;

        private List<GameObject> menuItems = new();

        private int selectedItem = 0;

        private void Awake()
        {
            if (GameInstance.Instance == null) return;

            foreach (Transform child in transform)
            {
                menuItems.Add(child.gameObject);
            }

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
            switch (selectedItem)
            {
                case 0:
                    charactersUI.enabled = true;
                    break;
                case 1:
                    playerInventoryUI.EnableInputs();
                    RemoveInputs();
                    charactersUI.enabled = false;
                    itemInfo.Show();
                    break;
            }
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

                switch (selectedItem)
                {
                    case 0:
                        charactersUI.enabled = true;
                        playerInventoryUI.Hide();
                        break;
                    case 1:
                        charactersUI.enabled = false;
                        playerInventoryUI.Show();
                        break;
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
