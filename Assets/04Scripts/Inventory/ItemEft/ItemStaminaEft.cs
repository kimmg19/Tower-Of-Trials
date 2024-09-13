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
            if (playerStats.maxStamina < 100)
            {
                playerStats.maxStamina += staminaPoint;
                //playerStats.maxStamina = Mathf.Clamp(playerStats.maxStamina, 50, 100); // 최대 스태미나를 100으로 제한
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
