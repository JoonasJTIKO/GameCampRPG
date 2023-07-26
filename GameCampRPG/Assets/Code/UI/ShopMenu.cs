using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace GameCampRPG.UI
{
    public class ShopMenu : MenuCanvas
    {
        [SerializeField]
        private GameObject shopItem;

        private ItemVendor itemVendor;

        private BlackMarketVendor blackVendor;

        private BaseVendor baseVendor;

        private PlayerInputs inputs;

        private InputAction menuUp;

        private InputAction menuDown;

        private InputAction select;

        private List<GameObject> shopItems = new();

        private List<Item> shopItemData;

        private float positionOffset = 50f;

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
            select.performed += SelectPerformed;
            menuUp.performed += MoveInMenusUp;
            menuDown.performed += MoveInMenusDown;

            GetComponentInChildren<TextMeshProUGUI>().text = "Money: " + GameInstance.Instance.GetPlayerInfo().Money.ToString() + "G";

            itemVendor = baseVendor.gameObject.GetComponent<ItemVendor>();
            blackVendor = baseVendor.gameObject.GetComponent<BlackMarketVendor>();
            if (itemVendor != null)
            {
                shopItemData = itemVendor.Inventory.ShowAllItems();
                DrawMenuList(itemVendor);
            }
            else if (blackVendor != null)
            {
                shopItemData = blackVendor.Inventory.ShowAllItems();
                DrawMenuList(blackVendor);
            }
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
                selectedItem = shopItems.Count - 1;
            }
            UpdateSelectedItem();
        }

        private void MoveInMenusDown(InputAction.CallbackContext callback)
        {
            if (selectedItem < shopItems.Count - 1)
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
            for (int i = 0; i < shopItems.Count; i++)
            {
                TextMeshProUGUI[] texts = shopItems[i].GetComponentsInChildren<TextMeshProUGUI>();

                if (i == selectedItem)
                {
                    texts[0].color = Color.red;
                    if (i < shopItemData.Count)
                    {
                        GameInstance.Instance.GetDialogueCanvas().SetText(shopItemData[i].Description);
                    }
                }
                else
                {
                    if (i < shopItemData.Count)
                    {
                        if (shopItemData[i].Price > GameInstance.Instance.GetPlayerInfo().Money)
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
            if (selectedItem == shopItems.Count - 1)
            {
                GameInstance.Instance.GetDialogueCanvas().StartDialogue(baseVendor, baseVendor.DialogueLines[^1]);
                Hide();
                return;
            }

            Item item;
            if (itemVendor != null)
            {
                item = itemVendor.Inventory.GetItem(shopItemData[selectedItem]);
                if (!BuyItem(item))
                {
                    itemVendor.Inventory.AddItems(item);
                }
                shopItemData = itemVendor.Inventory.ShowAllItems();
                DrawMenuList(itemVendor);
            }
            else if (blackVendor != null)
            {
                item = blackVendor.Inventory.GetItem(shopItemData[selectedItem]);
                if (!BuyItem(item))
                {
                    blackVendor.Inventory.AddItems(item);
                }
                shopItemData = blackVendor.Inventory.ShowAllItems();
                DrawMenuList(blackVendor);
            }
            UpdateSelectedItem();
        }

        private bool BuyItem(Item item)
        {
            Debug.Log("price: " + item.Price);
            Debug.Log("Money: " + GameInstance.Instance.GetPlayerInfo().Money);
            if (item.Price > GameInstance.Instance.GetPlayerInfo().Money)
            {
                return false;
            }

            GameInstance.Instance.GetPlayerInfo().AddItemToInventory(item);
            GameInstance.Instance.GetPlayerInfo().RemoveMoney(item.Price);
            GetComponentInChildren<TextMeshProUGUI>().text = "Money: " + GameInstance.Instance.GetPlayerInfo().Money.ToString() + "G";
            return true;
        }

        private void DrawMenuList(BaseVendor vendor)
        {
            if (shopItemData.Count == 0)
            {
                Hide();
                GameInstance.Instance.GetDialogueCanvas().StartDialogue(vendor, vendor.DialogueLines[^2]);
            }

            if (shopItems.Count != 0)
            {
                foreach (GameObject shopItem in shopItems)
                {
                    Destroy(shopItem);
                }

                shopItems = new List<GameObject>();
            }

            Vector3 shopItemPos = shopItem.GetComponent<RectTransform>().anchoredPosition3D;
            Vector3 newPosition;

            for (int i = 0; i < shopItemData.Count; i++)
            {
                newPosition = new(shopItemPos.x, shopItemPos.y - (positionOffset * i), shopItemPos.z);
                GameObject item = GameObject.Instantiate(shopItem, this.gameObject.transform);
                shopItems.Add(item);
                shopItems[i].GetComponent<RectTransform>().anchoredPosition3D = newPosition;
                TextMeshProUGUI[] texts = shopItems[i].GetComponentsInChildren<TextMeshProUGUI>();
                if (texts[0].gameObject.name == "ItemName")
                {
                    if (shopItemData[i].Amount > 1)
                    {
                        texts[0].text = shopItemData[i].Name + " x" + shopItemData[i].Amount.ToString();
                    }
                    else
                    {
                        texts[0].text = shopItemData[i].Name;
                    }
                }

                if (texts[1].gameObject.name == "Price")
                {
                    texts[1].text = shopItemData[i].Price.ToString() + "G";
                }
            }
            GameObject cancel = GameObject.Instantiate(shopItem, this.gameObject.transform);
            shopItems.Add(cancel);
            shopItems[^1].GetComponent<RectTransform>().anchoredPosition3D = new(shopItemPos.x, -410, shopItemPos.z);
            TextMeshProUGUI[] textComps = shopItems[^1].GetComponentsInChildren<TextMeshProUGUI>();
            if (textComps[0].gameObject.name == "ItemName")
            {
                textComps[0].text = "Cancel";
            }

            if (textComps[1].gameObject.name == "Price")
            {
                textComps[1].text = "";
            }
        }

        public void OpenShopMenu(BaseVendor baseVendor)
        {
            this.baseVendor = baseVendor;
            Show();
        }
    }
}
