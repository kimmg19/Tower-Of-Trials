using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwordSlot : MonoBehaviour, IPointerClickHandler
{
    public Item item; // ������ ���� ������
    public Image itemIcon; // ���� ������
    private PlayerStats playerStats; // �÷��̾��� ���� ����

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();

        if (item != null && playerStats != null)
        {
  
            bool isUse = item.Use(playerStats);

            if (isUse)
            {
                //Debug.Log("Sword equipped and effect applied.");
                // ���� �� �κ��丮���� ������ ���� ����
                // Inventory.instance.RemoveItem(item.slotNum);
            }
            else
            {
                //Debug.LogWarning("Failed to equip sword.");
            }
        }
    }

   

    public void UpdateSwordSlotUI()
    {
        if (itemIcon == null)
        {
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

    public void OnPointerClick(PointerEventData eventData)
    {
    }
}
