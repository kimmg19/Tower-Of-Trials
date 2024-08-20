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
        else
        {
            //Debug.Log("PlayerStats successfully found in the scene.");
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
        // 로그 추가로 디버깅
        Debug.Log("OnPointerUp called. Checking item and playerStats...");

        // item이 null인지 확인
        if (item == null)
        {
            Debug.Log("No item assigned to this slot. No action will be taken.");
            return; // 아이템이 없으면 아무 작업도 수행하지 않고 메서드를 종료
        }

        // playerStats가 null인지 확인
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats is not assigned.");
            return; // PlayerStats가 없으면 메서드를 종료
        }

        // 아이템을 사용해 보려고 시도
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
