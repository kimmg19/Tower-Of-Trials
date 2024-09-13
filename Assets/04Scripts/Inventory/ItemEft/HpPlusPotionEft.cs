using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/HpPotionPlus")]
public class HpPlusPotionEft : ItemEffect
{
    public int HpPlusPoint = 0; // Hp 포션 회복량 증가 수치
    public override bool ExecuteRole(PlayerStats playerStats)
    {
        if (playerStats != null)
        {
            if (playerStats.HpPotionRate < 50)
            {
                playerStats.HpPotionRate += HpPlusPoint;
                //playerStats.currentMp = Mathf.Clamp(playerStats.currentMp, 0, playerStats.maxMp); // 최대 마나를 초과하지 않도록 제한
                Debug.Log("Hp Potion 회복량 증가: " + HpPlusPoint + "%" + "총 회복량: " + playerStats.HpPotionRate);
                return true;
            }
            else
            {
                Debug.Log("Hp Potion 회복량이 이미 50임");
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