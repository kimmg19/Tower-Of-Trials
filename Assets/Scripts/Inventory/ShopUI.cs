using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopUI : MonoBehaviour
{
    public InventoryUI inventoryUI; // Editor���� ������ �� �ֵ��� ���� �ʵ�� ����
    Inventory inven;
    public Slot[] slots;
    bool activeShop = false;
    public GameObject invenShopPanel;

    void Start()
    {
        invenShopPanel.SetActive(activeShop);
        inven = Inventory.instance;
        inventoryUI = GetComponentInParent<Canvas>().GetComponentInChildren<InventoryUI>();

        // inventoryUI�� �����Ϳ��� �����Ǿ����� Ȯ��
        if (inventoryUI == null)
        {
            Debug.LogError("InventoryUI reference is missing!");
            return;
        }

        slots = inventoryUI.slotHolder.GetComponentsInChildren<Slot>();
        inven.onSlotCountChange += inventoryUI.SlotChange;
        inven.onChangeItem += inventoryUI.RedrawSlotUI;
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
    }
}
