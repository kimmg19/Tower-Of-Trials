using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/Mana")]
public class ItemManaEft : ItemEffect
{
    public int manaPoint = 0;

    public override bool ExecuteRole(PlayerStats playerStats)
    {
        if (playerStats != null)
        {
            if (playerStats.currentMp < playerStats.maxMp)
            {
                playerStats.currentMp += manaPoint;
                playerStats.currentMp = Mathf.Clamp(playerStats.currentMp, 0, playerStats.maxMp); // �ִ� ������ �ʰ����� �ʵ��� ����
                Debug.Log("Player MP Add: " + manaPoint);
                return true;
            }
            else
            {
                Debug.Log("Player MP is already full.");
                return false;
            }
        }
        else
        {
            Debug.LogError("PlayerStats is null!");
            return false;
        }
    }
}
