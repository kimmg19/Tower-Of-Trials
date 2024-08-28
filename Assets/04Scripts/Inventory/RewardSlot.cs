using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardSlot : MonoBehaviour
{
    [SerializeField]
    private GameObject RewardList;

    public Item item;
    public Image itemIcon;
    public Button buyButton;
    public PlayerStats playerstats;
    public RewardChest rewardchest;

    void Start()
    {
        buyButton.onClick.AddListener(OnCheckButtonClick);
    }

    void OnCheckButtonClick()
    {
        Inventory inventory = Inventory.instance;

        bool added = inventory.AddItem(item);

        if (added)
        {
            playerstats.Gold += rewardchest.RewardGold / 8;
            RewardList.SetActive(false);
            playerstats.OnApplicationQuit();
            // 아이템이 인벤토리에 성공적으로 추가된 경우
            Debug.Log("Item added to inventory: " + item.itemName);
            // 추가로 아이템을 슬롯에서 제거하거나 상점 재고에서 줄이는 등의 로직 추가 가능
        }
        else
        {
            Debug.Log("Failed to add item to inventory. Inventory might be full.");
        }
    }
}
