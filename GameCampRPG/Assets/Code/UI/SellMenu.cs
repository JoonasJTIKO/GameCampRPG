using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameCampRPG.UI
{
    public class SellMenu : MenuCanvas
    {
        [SerializeField]
        private GameObject listItem;

        private BaseVendor baseVendor;

        private PlayerInputs inputs;

        private InputAction menuUp;

        private InputAction menuDown;

        private InputAction select;

        private List<GameObject> listItems = new();

        private List<Item> listItemData;

        private PlayerInfo playerInfo;

        private float positionOffset = 50f;

        private int selectedItem = 0;

        private void Awake()
        {
            if (GameInstance.Instance == null) return;

            inputs = GameInstance.Instance.GetPlayerInfo().PlayerInputs;
            menuUp = inputs.UI.MenuUp;
            menuDown = inputs.UI.MenuDown;
            select = inputs.UI.Select;
            playerInfo = GameInstance.Instance.GetPlayerInfo();
        }

        private void OnEnable()
        {
            if (inputs == null) return;

            selectedItem = 0;
            select.performed += SelectPerformed;
            menuUp.performed += MoveInMenusUp;
            menuDown.performed += MoveInMenusDown;

            GetComponentInChildren<TextMeshProUGUI>().text = "Money: " + GameInstance.Instance.GetPlayerInfo().Money.ToString() + "G";

            listItemData = playerInfo.PlayerInventory.ShowAllItems();
            DrawMenuList();
            
            UpdateSelectedItem();
        }

        private void OnDisable()
        {
            if (inputs == null) return;

            select.performed -= SelectPerformed;
            menuUp.performed -= MoveInMenusUp;
            menuDown.performed -= MoveInMenusDown;
        }

        private void MoveInMenusUp(InputAction.CallbackContext callback)
        {
            if (selectedItem > 0)
            {
                selectedItem--;
            }
            else
            {
                selectedItem = listItems.Count - 1;
            }
            UpdateSelectedItem();
        }

        private void MoveInMenusDown(InputAction.CallbackContext callback)
        {
            if (selectedItem < listItems.Count - 1)
            {
                selectedItem++;
            }
            else
            {
                selectedItem = 0;
            }
            UpdateSelectedItem();
        }

        private void UpdateSelectedItem()
        {
            for (int i = 0; i < listItems.Count; i++)
            {
                TextMeshProUGUI[] texts = listItems[i].GetComponentsInChildren<TextMeshProUGUI>();

                if (i == selectedItem)
                {
                    texts[0].color = Color.red;
                    if (i < listItemData.Count)
                    {
                        GameInstance.Instance.GetDialogueCanvas().SetText(listItemData[i].Description);
                    }
                }
                else
                {
                    if (i < listItemData.Count)
                    {
                        if (listItemData[i].Price > GameInstance.Instance.GetPlayerInfo().Money)
                        {
                            texts[0].color = new Color(0, 0, 0, 0.5f);
                            texts[1].color = new Color(0, 0, 0, 0.5f);
                        }
                        else
                        {
                            texts[0].color = Color.black;
                            texts[1].color = Color.black;
                        }
                    }
                    else
                    {
                        texts[0].color = Color.black;
                    }
                }
            }
        }

        private void SelectPerformed(InputAction.CallbackContext callback)
        {
            Debug.Log("Action performed");
            if (selectedItem == listItems.Count - 1)
            {
                GameInstance.Instance.GetDialogueCanvas().StartDialogue(baseVendor, baseVendor.DialogueLines[^1]);
                Hide();
                return;
            }

            Item item;
            item = playerInfo.PlayerInventory.GetItem(listItemData[selectedItem]);
            if (!SellItem(item))
            {
                playerInfo.PlayerInventory.AddItems(item);
            }
            listItemData = playerInfo.PlayerInventory.ShowAllItems();
            DrawMenuList();

            UpdateSelectedItem();
        }

        private bool SellItem(Item item)
        {
            GameInstance.Instance.GetPlayerInfo().RemoveItem(item);
            GameInstance.Instance.GetPlayerInfo().AddMoney(item.Price);
            GetComponentInChildren<TextMeshProUGUI>().text = "Money: " + GameInstance.Instance.GetPlayerInfo().Money.ToString() + "G";
            return true;
        }

        private void DrawMenuList()
        {
            if (listItems.Count != 0)
            {
                foreach (GameObject listItem in listItems)
                {
                    Destroy(listItem);
                }

                listItems = new List<GameObject>();
            }

            Vector3 listItemPos = listItem.GetComponent<RectTransform>().anchoredPosition3D;
            Vector3 newPosition;

            for (int i = 0; i < listItemData.Count; i++)
            {
                newPosition = new(listItemPos.x, listItemPos.y - (positionOffset * i), listItemPos.z);
                GameObject item = GameObject.Instantiate(listItem, this.gameObject.transform);
                listItems.Add(item);
                listItems[i].GetComponent<RectTransform>().anchoredPosition3D = newPosition;
                TextMeshProUGUI[] texts = listItems[i].GetComponentsInChildren<TextMeshProUGUI>();
                if (texts[0].gameObject.name == "ItemName")
                {
                    if (listItemData[i].Amount > 1)
                    {
                        texts[0].text = listItemData[i].Name + " x" + listItemData[i].Amount.ToString();
                    }
                    else
                    {
                        texts[0].text = listItemData[i].Name;
                    }
                }

                if (texts[1].gameObject.name == "Price")
                {
                    texts[1].text = listItemData[i].Price.ToString() + "G";
                }
            }
            GameObject cancel = GameObject.Instantiate(listItem, this.gameObject.transform);
            listItems.Add(cancel);
            listItems[^1].GetComponent<RectTransform>().anchoredPosition3D = new(listItemPos.x, -410, listItemPos.z);
            TextMeshProUGUI[] textComps = listItems[^1].GetComponentsInChildren<TextMeshProUGUI>();
            if (textComps[0].gameObject.name == "ItemName")
            {
                textComps[0].text = "Cancel";
            }

            if (textComps[1].gameObject.name == "Price")
            {
                textComps[1].text = "";
            }
        }

        public void OpenSellMenu(BaseVendor vendor)
        {
            baseVendor = vendor;
            Show();
        }
    }
}
