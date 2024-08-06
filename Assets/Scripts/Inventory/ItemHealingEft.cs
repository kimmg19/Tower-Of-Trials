using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/Health")]
public class ItemHealthEft : ItemEffect
{
    public int healingPoint = 0;

    public override bool ExecuteRole(PlayerStats playerStats)
    {
        if (playerStats != null)
        {
            if (playerStats.currentHp < playerStats.maxHp)
            {
                playerStats.currentHp += healingPoint;
                playerStats.currentHp = Mathf.Clamp(playerStats.currentHp, 0, playerStats.maxHp); // 최대 체력을 초과하지 않도록 제한
                Debug.Log("Player HP Add: " + healingPoint);
                return true;
            }
            else
            {
                Debug.Log("Player HP is already full.");
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
