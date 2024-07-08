using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerUpHandler
{
    public int slotnum;
    public Item item;
    public Image itemIcon;
    private PlayerStats playerStats;

    void Start()
    {
        // PlayerStats 객체를 가져옵니다.
        playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats not found in the scene!");
        }
    }

    public void UpdateSlotUI()
    {
        if (item != null)
        {
            itemIcon.sprite = item.itemImage;
            itemIcon.gameObject.SetActive(true);
        }
        else
        {
            itemIcon.gameObject.SetActive(false);
        }
    }

    public void RemoveSlot()
    {
        item = null;
        itemIcon.gameObject.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (item != null && playerStats != null)
        {
            bool isUse = item.Use(playerStats);
            if (isUse)
            {
                Inventory.instance.RemoveItem(slotnum);
                Inventory.instance.SaveInventory();
            }
        }
        else
        {
            if (item == null)
            {
                Debug.LogError("Item is not assigned.");
            }
            if (playerStats == null)
            {
                Debug.LogError("PlayerStats is not assigned.");
            }
        }
    }
}
