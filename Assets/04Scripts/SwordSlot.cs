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

        if (playerStats == null)
        {
            //Debug.LogError("PlayerStats not found in the scene!");
        }

        if (item != null && playerStats != null)
        {
            // ���⸦ �����ϰ� ȿ�� ����
            //Debug.Log("Equipping sword...");
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

    /*
    public void EquipSword()
    {
        if (item != null && playerStats != null)
        {
            // ���⸦ �����ϰ� ȿ�� ����
            Debug.Log("Equipping sword...");
            bool isUse = item.Use(playerStats);

            if (isUse)
            {
                Debug.Log("Sword equipped and effect applied.");
                // ���� �� �κ��丮���� ������ ���� ����
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

    // ���� ���� �� UI ������Ʈ
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

    // Ŭ�� �� ���� ����
    public void OnPointerClick(PointerEventData eventData)
    {
        //EquipSword(); // �������� Ŭ���ϸ� ����
    }
}
