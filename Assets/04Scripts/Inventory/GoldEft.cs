using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/Gold")]
public class GoldEft : ItemEffect
{
    public int GoldPoint = 0;

    public override bool ExecuteRole(PlayerStats playerStats)
    {
        if (playerStats != null)
        {
            playerStats.Gold += GoldPoint;
            playerStats.OnApplicationQuit();
            playerStats.upgrade.SaveWeaponEnhancePoint();
            return true;
        }
        else
        {
            Debug.LogError("PlayeStats is null!");
            return false;
        }
    }
}