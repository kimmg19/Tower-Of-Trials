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
            RedrawSlotUI(); // ���� UI�� ���� �� ���� UI ����
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

    // ���õ� �������� �κ��丮�� �߰��ϴ� �Լ�
    public void OnBuyButtonPressed()
    {
        if (selectedItem != null)
        {
            bool success = inven.AddItem(selectedItem); // �κ��丮�� ������ �߰� �õ�
            if (success)
            {
                Debug.Log("Item added to inventory: " + selectedItem.itemName);
                // �������� ������ ��� ���� �� �߰� ������ ���⿡ �߰� ����
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
