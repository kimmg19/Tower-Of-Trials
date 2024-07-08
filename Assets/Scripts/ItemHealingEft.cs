using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/Health")]
public class ItemHealingEft : ItemEffect
{
    public int healingPoint = 0;

    public override bool ExecuteRole(PlayerStats playerStats)
    {
        if (playerStats != null)
        {
            playerStats.currentHp += healingPoint;
            Debug.Log("PlayerHp Add: " + healingPoint);
            return true;
        }
        else
        {
            Debug.LogError("PlayerStats is null!");
            return false;
        }
    }
}