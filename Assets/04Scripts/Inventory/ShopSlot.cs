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
            itemIcon.sprite = item.itemImage;  // ���Կ� ������ �̹��� ����
        }

        // ���� ��ư�� onClick �̺�Ʈ ������ �߰�
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
                // �� Item ��ü�� �����Ͽ� �κ��丮�� �߰�
                Item newItem = new Item
                {
                    itemName = item.itemName,
                    itemImage = item.itemImage,
                    quantity = 1, // �⺻ ������ 1�� ����
                    StorePrice = item.StorePrice,
                    efts = new List<ItemEffect>(item.efts)
                };

                bool added = inventory.AddItem(newItem);

                if (added)
                {
                    playerStats.Gold -= itemPrice;
                    playerStats.OnApplicationQuit();
                    upgrade.SaveWeaponEnhancePoint();
                }
     
            }
  
        }
  
    }


 
}
