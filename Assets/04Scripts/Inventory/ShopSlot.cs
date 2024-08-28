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
        // 인벤토리 참조
        Inventory inventory = Inventory.instance;

        if (playerStats != null)
        {
            // 아이템의 가격을 가져옴
            int itemPrice = item.StorePrice;

            // 플레이어의 골드가 충분한지 확인
            if (playerStats.Gold >= itemPrice)
            {
                // 인벤토리에 아이템 추가 시도
                bool added = inventory.AddItem(item);

                if (added)
                {
                    // 아이템이 인벤토리에 성공적으로 추가된 경우
                    playerStats.Gold -= itemPrice; // 플레이어의 골드에서 아이템 가격만큼 차감
                    Debug.Log("Item added to inventory: " + item.itemName);
                    Debug.Log("Remaining Gold: " + playerStats.Gold + " G");
                    playerStats.OnApplicationQuit();
                    upgrade.SaveWeaponEnhancePoint();
                    // 추가로 아이템을 슬롯에서 제거하거나 상점 재고에서 줄이는 등의 로직 추가 가능
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
