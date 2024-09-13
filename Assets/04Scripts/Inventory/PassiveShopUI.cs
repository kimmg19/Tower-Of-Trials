using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class PassiveShopUI : MonoBehaviour
{
    Inventory inven;
    public PlayerStats playerstats;
    public Slot[] slots;
    public GameObject invenPassiveShopPanel;
    public Transform slotHolder;
    bool activePassiveShop = false;

    private Item selectedItem;
    public Text PassiveGoldText;

    void Start()
    {
        inven = Inventory.instance;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        inven.onSlotCountChange += SlotChange;
        inven.onChangeItem += RedrawSlotUI;
        invenPassiveShopPanel.SetActive(activePassiveShop);
    }

    private void Update()
    {
        PassiveGoldText.text = playerstats.Gold + " G";
    }

    public void OnTogglePassiveShop(InputAction.CallbackContext context)
    {
        Debug.Log("TogglePassiveShop triggered");
        if (context.performed)
        {
            TogglePassiveShop();
        }
    }

    public void TogglePassiveShop()
    {
        activePassiveShop = !activePassiveShop;
        invenPassiveShopPanel.SetActive(activePassiveShop);

        if (activePassiveShop)
        {
            RedrawSlotUI(); // 상점 UI가 열릴 때 슬롯 UI 갱신
        }
    }

    public void SlotChange(int val)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].slotnum = i;

            if (i < inven.SlotCnt)
                slots[i].GetComponent<Button>().interactable = true;
            else
                slots[i].GetComponent<Button>().interactable = false;
        }
    }

    public void RedrawSlotUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveSlot();
        }
        for (int i = 0; i < inven.items.Count; i++)
        {
            slots[i].item = inven.items[i];
            slots[i].UpdateSlotUI();
        }
    }

    public void SelectItem(Item item)
    {
        selectedItem = item;
        Debug.Log("Selected Item: " + selectedItem.itemName);
    }


    public void OnClickCloseButton()
    {
        activePassiveShop = !activePassiveShop;
        invenPassiveShopPanel.SetActive(activePassiveShop);
    }
}