using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipment,
    Consumables,
    Etc
}

[System.Serializable]
public class Item
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemImage;
    public List<ItemEffect> efts;

    public int StorePrice; // 아이템의 가격

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    // JSON에서 역직렬화하기 위한 정적 메서드
    public static Item FromJson(string json)
    {
        return JsonUtility.FromJson<Item>(json);
    }

    public bool Use(PlayerStats playerStats)
    {
        bool isUsed = false;
        foreach (ItemEffect eft in efts)
        {
            isUsed = eft.ExecuteRole(playerStats);
        }

        return isUsed;
    }
}
