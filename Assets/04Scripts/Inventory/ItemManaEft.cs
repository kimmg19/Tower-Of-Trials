using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/Mana")]
public class ItemManaEft : ItemEffect
{
    public int manaPoint = 0; // 마나 회복수치

    public override bool ExecuteRole(PlayerStats playerStats)
    {
        if (playerStats != null)
        {
            if (playerStats.currentMp < playerStats.maxMp)
            {
                playerStats.currentMp += manaPoint;
                playerStats.currentMp = Mathf.Clamp(playerStats.currentMp, 0, playerStats.maxMp); // 최대 마나를 초과하지 않도록 제한
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
