using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

namespace GameCampRPG.UI
{
    public class PlayerInventoryUI : MenuCanvas
    {
        [SerializeField]
        private PauseMenuNavigation pauseMenuNavigation;

        [SerializeField]
        private ItemInfo itemInfo;

        [SerializeField]
        private GameObject itemFrame;

        private List<GameObject> inventoryMenuItems = new();

        private ItemEquipping itemEquipping;

        private List<Item> playerItems;

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
            playerItems = GameInstance.Instance.GetPlayerInfo().PlayerInventory.ShowAllItems();
            itemEquipping = GameInstance.Instance.GetItemEquipping();

            for (int i = 0; i < 25; i++)
            {
                GameObject menuFrame = GameObject.Instantiate(itemFrame, this.transform);
                inventoryMenuItems.Add(menuFrame);
                if (i < playerItems.Count)
                {
                    inventoryMenuItems[i].GetComponentsInChildren<TextMeshProUGUI>()[0].text = playerItems[i].Amount.ToString();
                    //inventoryMenuItems[i].GetComponentInChildren<Image>().sprite = playerItems[i].Icon;
                }
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
            Debug.Log("Select performed");
        }

        private void EscapePerformed(InputAction.CallbackContext callback)
        {
            selectedItem = -1;
            UpdateSelectedItem();
            itemInfo.Hide();
            DisableInputs();
            pauseMenuNavigation.SetInputs();
        }

        private void UpdateSelectedItem()
        {
            if (selectedItem < playerItems.Count && selectedItem >= 0)
            {
                itemInfo.SetInfo(playerItems[selectedItem]);
            }
            else
            {
                itemInfo.ClearInfo();
            }

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
            IndicateEquippedItems();
        }

        private void IndicateEquippedItems()
        {
            for (int i = 0; i < playerItems.Count; i++)
            {
                if (playerItems[i] == itemEquipping.EquippedKnightWeapon || playerItems[i] == itemEquipping.EquippedKnightArmor)
                {
                    inventoryMenuItems[i].GetComponentsInChildren<TextMeshProUGUI>()[1].text = "E";
                    inventoryMenuItems[i].GetComponentsInChildren<TextMeshProUGUI>()[1].color = Color.blue;
                }
                else if (playerItems[i] == itemEquipping.EquippedRogueWeapon || playerItems[i] == itemEquipping.EquippedRogueArmor)
                {
                    inventoryMenuItems[i].GetComponentsInChildren<TextMeshProUGUI>()[1].text = "E";
                    inventoryMenuItems[i].GetComponentsInChildren<TextMeshProUGUI>()[1].color = Color.green;
                }
                else if (playerItems[i] == itemEquipping.EquippedMageWeapon || playerItems[i] == itemEquipping.EquippedMageArmor)
                {
                    inventoryMenuItems[i].GetComponentsInChildren<TextMeshProUGUI>()[1].text = "E";
                    inventoryMenuItems[i].GetComponentsInChildren<TextMeshProUGUI>()[1].color = Color.magenta;
                }
            }
        }
    }
}
