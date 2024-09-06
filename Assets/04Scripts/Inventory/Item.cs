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
    public int quantity; // 아이템의 수량 추가

    // 기본 생성자
    public Item()
    {
        quantity = 1; // 기본 수량을 1로 설정
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

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