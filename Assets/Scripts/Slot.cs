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
        // PlayerStats 객체를 찾습니다.
        playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats not found in the scene!");
        }
    }

    public void UpdateSlotUI()
    {
        if (itemIcon == null)
        {
            Debug.LogError("ItemIcon is not assigned.");
            return;
        }

        if (item != null)
        {
            if (item.itemImage != null)
            {
                // 스프라이트가 파괴되지 않았는지 확인
                if (item.itemImage != null)
                {
                    itemIcon.sprite = item.itemImage;
                    itemIcon.gameObject.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("Item's image is destroyed.");
                    itemIcon.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogWarning("Item's image is not assigned.");
                itemIcon.gameObject.SetActive(false);
            }
        }
        else
        {
            itemIcon.gameObject.SetActive(false);
        }
    }

    public void RemoveSlot()
    {
        item = null;
        if (itemIcon != null)
        {
            itemIcon.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("ItemIcon is not assigned.");
        }
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
