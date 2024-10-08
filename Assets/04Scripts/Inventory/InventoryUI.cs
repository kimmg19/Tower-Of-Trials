using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;
    GameObject player;
    PlayerInputs playerInputs;
    public GameObject inventoryPanel;
    bool activeInventory = false;
    public Slot[] slots;
    public Transform slotHolder;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerInputs = player.GetComponent<PlayerInputs>();
        inven = Inventory.instance;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        inven.onSlotCountChange += SlotChange;
        inven.onChangeItem += RedrawSlotUI;
        inventoryPanel.SetActive(activeInventory);
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

    public void OnToggleInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        playerInputs.isInteracting = !playerInputs.isInteracting;
        activeInventory = !activeInventory;
        inventoryPanel.SetActive(activeInventory);

        // 인벤토리 상태에 따라 오디오 재생
        if (activeInventory)
        {
            AudioManager.instance.Play("InventoryOpen"); // 인벤토리 열기 오디오 재생
        }
        else
        {
            AudioManager.instance.Play("InventoryClose"); // 인벤토리 닫기 오디오 재생
        }
    }


    public void AddSlot()
    {
        inven.SlotCnt++;
    }
    public void RemoveSlot()
    {
        inven.SlotCnt--;
    }

    public void RedrawSlotUI()
    {
        //Debug.Log("RedrawSlotUI called.");

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveSlot();
            //Debug.Log($"Slot {i} cleared.");
        }
        for (int i = 0; i < inven.items.Count; i++)
        {
            slots[i].item = inven.items[i];
            slots[i].UpdateSlotUI();
            Debug.Log($"Slot {i} updated with item '{slots[i].item.itemName}' (quantity: {slots[i].item.quantity}).");
        }
        //Debug.Log("RedrawSlotUI completed.");
    }
}