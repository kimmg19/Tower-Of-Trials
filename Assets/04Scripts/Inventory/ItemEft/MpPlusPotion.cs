using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/MpPotionPlus")]
public class MpPlusPotionEft : ItemEffect
{
    public int MpPlusPoint = 0; // Hp ���� ȸ���� ���� ��ġ
    public override bool ExecuteRole(PlayerStats playerStats)
    {
        if (playerStats != null)
        {
            if (playerStats.MpPotionRate < 50)
            {
                playerStats.MpPotionRate += MpPlusPoint;
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