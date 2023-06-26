using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;

namespace GameCampRPG.UI
{
    public class InteractionMenu : MenuCanvas
    {
        [SerializeField]
        private ShopMenu shopMenu;

        private PlayerInputs inputs;

        private InputAction menuUp;

        private InputAction menuDown;

        private InputAction select;

        private int selectedItem = 0;

        private List<TextMeshProUGUI> menuItems;

        private BaseVendor baseVendor;

        private ItemVendor itemVendor;

        private HealVendor healVendor;

        private BlackMarketVendor blackMarketVendor;

        private void Awake()
        {
            if (GameInstance.Instance == null) return;

            inputs = GameInstance.Instance.GetPlayerInfo().PlayerInputs;
            menuUp = inputs.UI.MenuUp;
            menuDown = inputs.UI.MenuDown;
            select = inputs.UI.Select;
            menuItems = gameObject.GetComponentsInChildren<TextMeshProUGUI>().ToList();
        }

        private void OnEnable()
        {
            if (inputs == null) return;

            selectedItem = 0;
            select.performed += SelectPerformed;
            menuUp.performed += MoveInMenusUp;
            menuDown.performed += MoveInMenusDown;
            SetMenuOptions();
            UpdateSelectedItem();
        }

        private void SetMenuOptions()
        {
            if (itemVendor != null)
            {
                menuItems[0].text = "Buy";
                menuItems[1].text = "Talk";
            }
            else if (healVendor != null)
            {
                menuItems[0].text = "Heal";
                menuItems[1].text = "Upgrade";
            }
            else if (blackMarketVendor != null)
            {
                menuItems[0].text = "Buy";
                menuItems[1].text = "Sell";
            }
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
            if (itemVendor != null)
            {
                switch (selectedItem)
                {
                    case 0:
                        Buy();
                        select.performed -= SelectPerformed;
                        break;
                    case 1:
                        Talk();
                        select.performed -= SelectPerformed;
                        break;
                    case 2:
                        Cancel();
                        select.performed -= SelectPerformed;
                        break;
                }
            }
            else if (healVendor != null)
            {
                switch (selectedItem)
                {
                    case 0:
                        Heal();
                        //select.performed -= SelectPerformed;
                        break;
                    case 1:
                        Upgrade();
                        //select.performed -= SelectPerformed;
                        break;
                    case 2:
                        Cancel();
                        select.performed -= SelectPerformed;
                        break;
                }
            }
            else if (blackMarketVendor != null)
            {
                switch (selectedItem)
                {
                    case 0:
                        Buy();
                        select.performed -= SelectPerformed;
                        break;
                    case 1:
                        Sell();
                        //select.performed -= SelectPerformed;
                        break;
                    case 2:
                        Cancel();
                        select.performed -= SelectPerformed;
                        break;
                }
            }
        }

        private void UpdateSelectedItem()
        {
            for (int i = 0; i < menuItems.Count; i++)
            {
                if (i == selectedItem)
                {
                    menuItems[i].color = Color.red;
                }
                else
                {
                    menuItems[i].color = Color.black;
                }
            }
        }

        private void Buy()
        {
            shopMenu.OpenShopMenu(baseVendor);
            Hide();
        }

        private void Talk()
        {
            GameInstance.Instance.GetDialogueCanvas().StartDialogue(baseVendor, baseVendor.DialogueLines[1], true);
            Hide();
        }

        private void Heal()
        {
            //TODO: Some kind of animation or such to indicate healing
            PlayerInfo playerInfo = GameInstance.Instance.GetPlayerInfo();
            for (int i = 0; i < playerInfo.CharacterHealths.Length; i++)
            {
                //TODO: If different player characters have different max healths, somehow override 3 with respective characters max health
                playerInfo.SetCharacterHealth(i, 3);
            }
            Debug.Log("Healing");
        }

        private void Upgrade()
        {
            Debug.Log("Open upgrade menu");
        }

        private void Sell()
        {
            Debug.Log("Open sell interface");
        }

        private void Cancel()
        {
            GameInstance.Instance.GetDialogueCanvas().StartDialogue(baseVendor, baseVendor.DialogueLines[^1]);
            Hide();
        }

        public void OpenMenu(BaseVendor caller)
        {
            GameObject vendor = caller.gameObject;
            baseVendor = caller;
            itemVendor = vendor.GetComponent<ItemVendor>();
            healVendor = vendor.GetComponent<HealVendor>();
            blackMarketVendor = vendor.GetComponent<BlackMarketVendor>();
            Show();
        }
    }
}
