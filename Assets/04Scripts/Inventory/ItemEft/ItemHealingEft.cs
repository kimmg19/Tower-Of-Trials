using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/Health")]
public class ItemHealthEft : ItemEffect
{
    public override bool ExecuteRole(PlayerStats playerStats)
    {
        if (playerStats != null)
        {
            int HealingAmount = Mathf.RoundToInt((playerStats.HpPotionRate / 100f) * playerStats.maxHp);

            if (playerStats.currentHp < playerStats.maxHp)
            {
                playerStats.currentHp += HealingAmount;
                playerStats.currentHp = Mathf.Clamp(playerStats.currentHp, 0, playerStats.maxHp); // �ִ� ü���� �ʰ����� �ʵ��� ����
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
