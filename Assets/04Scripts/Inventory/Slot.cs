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
       

        // itemIcon과 quantityText가 할당되지 않았다면, 자식에서 검색하여 초기화
        if (itemIcon == null)
        {
            itemIcon = GetComponentInChildren<Image>();
        }

        if (quantityText == null)
        {
            quantityText = GetComponentInChildren<Text>();
       
        }
    }


    public void UpdateSlotUI()
    {
        if (itemIcon == null)
        {
            return;
        }

        if (quantityText == null)
        {
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

        if (item == null)
        {
            return;
        }

        if (playerStats == null)
        {
            return;
        }

        try
        {
            bool isUse = item.Use(playerStats);
            if (isUse)
            {
                Inventory.instance.RemoveItem(slotnum);
                Inventory.instance.SaveInventory();
            }
   
       
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Exception occurred while using item: " + ex.Message);
        }
    }

}
