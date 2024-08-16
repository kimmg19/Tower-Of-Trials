using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopUI : MonoBehaviour
{
    Inventory inven;
    public Slot[] slots;
    public GameObject invenShopPanel;
    public Transform slotHolder;
    bool activeShop = false;

    private Item selectedItem;

    void Start()
    {
        inven = Inventory.instance;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        inven.onSlotCountChange += SlotChange;
        inven.onChangeItem += RedrawSlotUI;
        invenShopPanel.SetActive(activeShop);
    }

    public void OnToggleShop(InputAction.CallbackContext context)
    {
        Debug.Log("ToggleShop triggered");
        if (context.performed)
        {
            ToggleShop();
        }
    }

    public void ToggleShop()
    {
        activeShop = !activeShop;
        invenShopPanel.SetActive(activeShop);

        if (activeShop)
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

    // 선택된 아이템을 인벤토리에 추가하는 함수
    public void OnBuyButtonPressed()
    {
        if (selectedItem != null)
        {
            bool success = inven.AddItem(selectedItem); // 인벤토리에 아이템 추가 시도
            if (success)
            {
                Debug.Log("Item added to inventory: " + selectedItem.itemName);
                // 상점에서 아이템 재고 감소 등 추가 로직을 여기에 추가 가능
            }
            else
            {
                Debug.Log("Inventory is full or failed to add item.");
            }
        }
        else
        {
            Debug.Log("No item selected to buy.");
        }
    }

    public void OnClickCloseButton()
    {
        activeShop = !activeShop;
        invenShopPanel.SetActive(activeShop);
    }
}
