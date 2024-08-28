using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwordSlot : MonoBehaviour, IPointerClickHandler
{
    public Item item; // 장착할 무기 아이템
    public Image itemIcon; // 무기 아이콘
    private PlayerStats playerStats; // 플레이어의 스탯 정보

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();

        if (playerStats == null)
        {
            //Debug.LogError("PlayerStats not found in the scene!");
        }

        if (item != null && playerStats != null)
        {
            // 무기를 장착하고 효과 적용
            //Debug.Log("Equipping sword...");
            bool isUse = item.Use(playerStats);

            if (isUse)
            {
                //Debug.Log("Sword equipped and effect applied.");
                // 장착 후 인벤토리에서 제거할 수도 있음
                // Inventory.instance.RemoveItem(item.slotNum);
            }
            else
            {
                //Debug.LogWarning("Failed to equip sword.");
            }
        }
    }

    /*
    public void EquipSword()
    {
        if (item != null && playerStats != null)
        {
            // 무기를 장착하고 효과 적용
            Debug.Log("Equipping sword...");
            bool isUse = item.Use(playerStats);

            if (isUse)
            {
                Debug.Log("Sword equipped and effect applied.");
                // 장착 후 인벤토리에서 제거할 수도 있음
                // Inventory.instance.RemoveItem(item.slotNum);
            }
            else
            {
                Debug.LogWarning("Failed to equip sword.");
            }
        }
        else
        {
            Debug.LogWarning("No sword or PlayerStats available for equipping.");
        }
    }
    */

    // 무기 장착 시 UI 업데이트
    public void UpdateSwordSlotUI()
    {
        if (itemIcon == null)
        {
            Debug.LogError("ItemIcon is not assigned.");
            return;
        }

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

    // 클릭 시 무기 장착
    public void OnPointerClick(PointerEventData eventData)
    {
        //EquipSword(); // 아이템을 클릭하면 장착
    }
}
