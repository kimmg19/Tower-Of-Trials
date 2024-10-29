using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Equipment/Sword")]
public class SwordEft : ItemEffect
{
    public int SwordAttackPoint = 0; // ���� ��ȭ ��ġ

    public override bool ExecuteRole(PlayerStats playerStats)
    {
        if (playerStats != null)
        {
            playerStats.IncreaseSwordDamage(SwordAttackPoint);
            return true;
        }

        return false;
    }
}