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
        for(int i = 0; i < slots.Length; i++)
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
        for(int i = 0; i< slots.Length; i++)
        {
            slots[i].RemoveSlot();
        }
        for(int i = 0; i < inven.items.Count; i++)
        {
            slots[i].item = inven.items[i];
            slots[i].UpdateSlotUI();
        }
    }
}