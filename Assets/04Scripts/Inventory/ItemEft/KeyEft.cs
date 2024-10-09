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
            // 여기에 KeyEft가 실행할 로직을 추가
            Debug.Log("KeyEft effect executed.");
            return true;
        }
        else
        {
            Debug.LogError("PlayerStats is null!");
            return false;
        }
    }
}
