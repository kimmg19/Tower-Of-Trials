using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/Stamina")]
public class ItemStaminaEft : ItemEffect
{
    public int staminaPoint = 0; // 스태미나 회복 수치

    public override bool ExecuteRole(PlayerStats playerStats)
    {
        if (playerStats != null)
        {
            if (playerStats.currentStamina < playerStats.maxStamina)
            {
                playerStats.currentStamina += staminaPoint;
                playerStats.currentStamina = Mathf.Clamp(playerStats.currentStamina, 0, playerStats.maxStamina); // 최대 스태미나를 초과하지 않도록 제한
                Debug.Log("Player Stamina Add: " + staminaPoint);
                return true;
            }
            else
            {
                Debug.Log("Player Stamina is already full.");
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
