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

        private bool consumable = false;

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
                DrawMenu(new string[] { "Equip", "Unequip" });
                UpdateSelectedItem();
            }
            else
            {
                DrawMenu(new string[] { "Use" });
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
                    DrawMenu(EquipOptionsBasedOnItem(inventoryItem));
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
                    playerInventoryUI.EnableInputs();
                    RemoveInputs();
                    break;
                case "Use":
                    DrawMenu(EquipOptionsBasedOnItem(inventoryItem));
                    UpdateSelectedItem();
                    consumable = true;
                    break;
                case "Knight":
                    if (consumable)
                    {
                        GameInstance.Instance.GetPlayerInfo().UseItem(inventoryItem.UseType, 1);
                        GameInstance.Instance.GetPlayerInfo().PlayerInventory.GetItem(inventoryItem);
                        playerInventoryUI.EnableInputs();
                        RemoveInputs();
                        consumable = false;
                    }
                    else
                    {
                        itemEquipping.EquipItem(inventoryItem, 1);
                        playerInventoryUI.EnableInputs();
                        RemoveInputs();
                    }
                    break;
                case "Rogue":
                    if (consumable)
                    {
                        GameInstance.Instance.GetPlayerInfo().UseItem(inventoryItem.UseType, 0);
                        GameInstance.Instance.GetPlayerInfo().PlayerInventory.GetItem(inventoryItem);
                        playerInventoryUI.EnableInputs();
                        RemoveInputs();
                        consumable = false;
                    }
                    else
                    {
                        itemEquipping.EquipItem(inventoryItem, 0);
                        playerInventoryUI.EnableInputs();
                        RemoveInputs();
                    }
                    break;
                case "Mage":
                    if (consumable)
                    {
                        GameInstance.Instance.GetPlayerInfo().UseItem(inventoryItem.UseType, 2);
                        GameInstance.Instance.GetPlayerInfo().PlayerInventory.GetItem(inventoryItem);
                        playerInventoryUI.EnableInputs();
                        RemoveInputs();
                        consumable = false;
                    }
                    else
                    {
                        itemEquipping.EquipItem(inventoryItem, 2);
                        playerInventoryUI.EnableInputs();
                        RemoveInputs();
                    }
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

        private void DrawMenu(string[] menuItemNames)
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

            for (int i = 0; i < menuItemNames.Length; i++)
            {
                newPosition = new(optionPosition.x, optionPosition.y - (225 * i), optionPosition.z);
                GameObject option = GameObject.Instantiate(menuItem, this.gameObject.transform);
                menuItems.Add(option);
                menuItems[i].GetComponent<RectTransform>().anchoredPosition3D = newPosition;
                menuItems[i].GetComponent<TextMeshProUGUI>().text = menuItemNames[i];
            }
            GameObject cancel = GameObject.Instantiate(menuItem, this.gameObject.transform);
            menuItems.Add(cancel);
            menuItems[^1].GetComponent<RectTransform>().anchoredPosition3D = new(optionPosition.x, optionPosition.y - (225 * menuItemNames.Length), optionPosition.z);
            menuItems[^1].GetComponent<TextMeshProUGUI>().text = "Cancel";
        }

        private string[] EquipOptionsBasedOnItem(Item item)
        {
            if (item.EquipCharacter == ItemEquipping.EquipCharacter.Knight)
            {
                return new string[] { "Knight" };
            }
            else if (item.EquipCharacter == ItemEquipping.EquipCharacter.Rogue)
            {
                return new string[] { "Rogue" };
            }
            else if (item.EquipCharacter == ItemEquipping.EquipCharacter.Mage)
            {
                return new string[] { "Mage" };
            }
            else
            {
                return new string[] { "Knight", "Rogue", "Mage" };
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
