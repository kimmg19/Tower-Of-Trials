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

    public Text RewardQuantityText;

    void Start()
    {
        buyButton.onClick.AddListener(OnCheckButtonClick);
        RewardQuantityText.text = item.quantity.ToString();
    }

    void OnCheckButtonClick()
    {
        Inventory inventory = Inventory.instance;

        Item newItem = new Item
        {
            itemName = item.itemName,
            itemImage = item.itemImage,
            quantity = item.quantity,
            StorePrice = item.StorePrice,
            efts = new List<ItemEffect>(item.efts) // ItemEffect ����Ʈ ����
        };

        bool added = inventory.AddItem(item);

        if (added)
        {
            //playerstats.Gold += rewardchest.RewardGold / 8;
            RewardList.SetActive(false);
            playerstats.OnApplicationQuit();
        }
   
    }
}
