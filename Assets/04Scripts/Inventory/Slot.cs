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

    public Text quantityText; // 아이템 수량을 표시할 UI Text

    void Start()
    {
        // PlayerStats 객체를 찾습니다.
        playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats not found in the scene!");
        }

        // itemIcon과 quantityText가 할당되지 않았다면, 자식에서 검색하여 초기화
        if (itemIcon == null)
        {
            itemIcon = GetComponentInChildren<Image>();
            if (itemIcon == null)
            {
                Debug.LogError("ItemIcon is not assigned in the Slot script and could not be found in children.");
            }
        }

        if (quantityText == null)
        {
            quantityText = GetComponentInChildren<Text>();
            if (quantityText == null)
            {
                Debug.LogError("QuantityText is not assigned in the Slot script and could not be found in children.");
            }
        }
    }


    public void UpdateSlotUI()
    {
        if (itemIcon == null)
        {
            Debug.LogError("ItemIcon가 할당되지 않았습니다.");
            return;
        }

        if (quantityText == null)
        {
            Debug.LogError("QuantityText가 할당되지 않았습니다.");
            return;
        }

        if (item != null)
        {
            if (item.itemImage != null)
            {
                itemIcon.sprite = item.itemImage;
                itemIcon.gameObject.SetActive(true);
                quantityText.text = item.quantity.ToString();
            }
            else
            {
                itemIcon.gameObject.SetActive(false);
                quantityText.text = "";
            }
        }
        else
        {
            itemIcon.gameObject.SetActive(false);
            quantityText.text = "";
        }
    }

    public void RemoveSlot()
    {
        item = null;
        if (itemIcon != null)
        {
            itemIcon.gameObject.SetActive(false);
        }
        quantityText.text = ""; // 아이템이 제거되면 수량도 초기화
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp called. Checking item and playerStats...");

        if (item == null)
        {
            Debug.Log("No item assigned to this slot. No action will be taken.");
            return;
        }

        if (playerStats == null)
        {
            Debug.LogError("PlayerStats is not assigned.");
            return;
        }

        try
        {
            bool isUse = item.Use(playerStats);
            if (isUse)
            {
                Debug.Log("Item used successfully. Removing item from inventory.");
                Inventory.instance.RemoveItem(slotnum);
                Inventory.instance.SaveInventory();
            }
            else
            {
                Debug.LogWarning("Item use failed.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Exception occurred while using item: " + ex.Message);
        }
    }

}
