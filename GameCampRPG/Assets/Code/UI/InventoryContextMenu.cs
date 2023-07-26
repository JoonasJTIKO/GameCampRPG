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

        [SerializeField]
        private PlayerInventoryUI playerInventoryUI;

        private ItemEquipping itemEquipping;

        private PlayerInputs inputs;

        private InputAction menuUp;

        private InputAction menuDown;

        private InputAction select;

        private List<GameObject> menuItems = new();

        private Item inventoryItem;

        private int selectedItem = 0;

        private void Awake()
        {
            if (GameInstance.Instance == null) return;

            inputs = GameInstance.Instance.GetPlayerInfo().PlayerInputs;
            itemEquipping = GetComponent<ItemEquipping>();
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
        }

        public void InitializeMenu(Item item)
        {
            inventoryItem = item;
            if (inventoryItem.Equippable)
            {
                DrawMenu(3, new string[] { "Equip", "Unequip" });
                UpdateSelectedItem();
            }
            else
            {
                DrawMenu(2, new string[] { "Use" });
                UpdateSelectedItem();
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
            string option = menuItems[selectedItem].GetComponent<TextMeshProUGUI>().text;
            switch (option)
            {
                case "Equip":
                    DrawMenu(4, new string[] { "Knight", "Rogue", "Mage" });
                    UpdateSelectedItem();
                    break;
                case "Unequip":
                    if (inventoryItem == itemEquipping.EquippedKnightArmor || inventoryItem == itemEquipping.EquippedKnightWeapon)
                    {
                        itemEquipping.UnequipItem(inventoryItem, 1);
                    }
                    else if (inventoryItem == itemEquipping.EquippedRogueArmor || inventoryItem == itemEquipping.EquippedRogueWeapon)
                    {
                        itemEquipping.UnequipItem(inventoryItem, 0);
                    }
                    else if (inventoryItem == itemEquipping.EquippedMageArmor || inventoryItem == itemEquipping.EquippedMageWeapon)
                    {
                        itemEquipping.UnequipItem(inventoryItem, 2);
                    }
                    break;
                case "Use":
                    break;
                case "Knight":
                    itemEquipping.EquipItem(inventoryItem, 1);
                    playerInventoryUI.EnableInputs();
                    RemoveInputs();
                    break;
                case "Rogue":
                    itemEquipping.EquipItem(inventoryItem, 0);
                    playerInventoryUI.EnableInputs();
                    RemoveInputs();
                    break;
                case "Mage":
                    itemEquipping.EquipItem(inventoryItem, 2);
                    playerInventoryUI.EnableInputs();
                    RemoveInputs();
                    break;
                case "Cancel":
                    playerInventoryUI.EnableInputs();
                    RemoveInputs();
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
