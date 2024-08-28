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
        // �κ��丮 ����
        Inventory inventory = Inventory.instance;

        if (playerStats != null)
        {
            // �������� ������ ������
            int itemPrice = item.StorePrice;

            // �÷��̾��� ��尡 ������� Ȯ��
            if (playerStats.Gold >= itemPrice)
            {
                // �κ��丮�� ������ �߰� �õ�
                bool added = inventory.AddItem(item);

                if (added)
                {
                    // �������� �κ��丮�� ���������� �߰��� ���
                    playerStats.Gold -= itemPrice; // �÷��̾��� ��忡�� ������ ���ݸ�ŭ ����
                    Debug.Log("Item added to inventory: " + item.itemName);
                    Debug.Log("Remaining Gold: " + playerStats.Gold + " G");
                    playerStats.OnApplicationQuit();
                    upgrade.SaveWeaponEnhancePoint();
                    // �߰��� �������� ���Կ��� �����ϰų� ���� ����� ���̴� ���� ���� �߰� ����
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
