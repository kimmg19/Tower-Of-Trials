using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/HpPotionPlus")]
public class HpPlusPotionEft : ItemEffect
{
    public int HpPlusPoint = 0; // Hp ���� ȸ���� ���� ��ġ
    public override bool ExecuteRole(PlayerStats playerStats)
    {
        if (playerStats != null)
        {
            if (playerStats.HpPotionRate < 50)
            {
                playerStats.HpPotionRate += HpPlusPoint;
                playerStats.OnApplicationQuit();
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