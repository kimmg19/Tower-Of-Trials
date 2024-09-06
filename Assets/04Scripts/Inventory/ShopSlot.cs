using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    public Item item;
    public Image itemIcon;
    public Button buyButton;
    private PlayerStats playerStats;
    public Upgrade upgrade;

    // Start is called before the first frame update
    void Start()
    {
        if (item != null)
        {
            itemIcon.sprite = item.itemImage;  // 슬롯에 아이템 이미지 설정
        }

        // 구매 버튼에 onClick 이벤트 리스너 추가
        buyButton.onClick.AddListener(OnBuyButtonClick);

        playerStats = FindObjectOfType<PlayerStats>();
    }

    void OnBuyButtonClick()
    {
        Inventory inventory = Inventory.instance;

        if (playerStats != null)
        {
            int itemPrice = item.StorePrice;

            if (playerStats.Gold >= itemPrice)
            {
                // 새 Item 객체를 생성하여 인벤토리에 추가
                Item newItem = new Item
                {
                    itemName = item.itemName,
                    itemImage = item.itemImage,
                    quantity = 1, // 기본 수량을 1로 설정
                    StorePrice = item.StorePrice,
                    efts = new List<ItemEffect>(item.efts)
                };

                bool added = inventory.AddItem(newItem);

                if (added)
                {
                    playerStats.Gold -= itemPrice;
                    Debug.Log("Item added to inventory: " + newItem.itemName);
                    Debug.Log("Remaining Gold: " + playerStats.Gold + " G");
                    playerStats.OnApplicationQuit();
                    upgrade.SaveWeaponEnhancePoint();
                }
                else
                {
                    Debug.Log("Failed to add item to inventory. Inventory might be full.");
                }
            }
            else
            {
                Debug.Log("Not enough gold to buy item: " + item.itemName + ". Required: " + itemPrice + " G");
            }
        }
        else
        {
            Debug.LogError("PlayerStats not found!");
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
