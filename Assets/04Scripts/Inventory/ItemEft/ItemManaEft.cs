using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/Mana")]
public class ItemManaEft : ItemEffect
{
    public override bool ExecuteRole(PlayerStats playerStats)
    {
        if (playerStats != null)
        {
            int ManaAmount = Mathf.RoundToInt((playerStats.MpPotionRate / 100f) * playerStats.maxMp);

            if (playerStats.currentMp < playerStats.maxMp)
            {
                playerStats.currentMp += ManaAmount;
                playerStats.currentMp = Mathf.Clamp(playerStats.currentMp, 0, playerStats.maxMp); // �ִ� ������ �ʰ����� �ʵ��� ����
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
