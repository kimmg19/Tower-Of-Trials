using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Equipment/Sword")]
public class SwordEft : ItemEffect
{
    public int SwordAttackPoint = 0; // 무기 강화 수치

    public override bool ExecuteRole(PlayerStats playerStats)
    {
        if (playerStats != null)
        {
            playerStats.IncreaseSwordDamage(SwordAttackPoint);
            Debug.Log("Sword damageAmount increased by " + SwordAttackPoint);
            return true;
        }

        return false;
    }
}