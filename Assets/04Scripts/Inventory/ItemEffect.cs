using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemEffect : ScriptableObject
{
    public int StorePrice; // 아이템 가격

    public abstract bool ExecuteRole(PlayerStats playerStats);
}
