using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    public Item item;
    public Image itemIcon;
    public Button buyButton;

    // Start is called before the first frame update
    void Start()
    {
        if (item != null)
        {
            itemIcon.sprite = item.itemImage;  // ���Կ� ������ �̹��� ����
        }

        // ���� ��ư�� onClick �̺�Ʈ ������ �߰�
        buyButton.onClick.AddListener(OnBuyButtonClick);
    }

    void OnBuyButtonClick()
    {
        // �κ��丮 ����
        Inventory inventory = Inventory.instance;

        // �κ��丮�� ������ �߰� �õ�
        bool added = inventory.AddItem(item);

        if (added)
        {
            Debug.Log("Item added to inventory: " + item.itemName);
            // �߰��� �������� ���Կ��� �����ϰų� ���� ����� ���̴� ���� ���� �߰� ����
        }
        else
        {
            Debug.Log("Failed to add item to inventory. Inventory might be full.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
