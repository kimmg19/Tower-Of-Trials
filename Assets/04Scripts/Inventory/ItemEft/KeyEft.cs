using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/Key")]
public class KeyEft : ItemEffect
{

    public override bool ExecuteRole(PlayerStats playerStats)
    {
        if (playerStats != null)
        {
            return true;
        }
        else
        {
            Debug.LogError("PlayeStats is null!");
            return false;
        }
    }
}