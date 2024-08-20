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
            itemIcon.sprite = item.itemImage;  // 슬롯에 아이템 이미지 설정
        }

        // 구매 버튼에 onClick 이벤트 리스너 추가
        buyButton.onClick.AddListener(OnBuyButtonClick);
    }

    void OnBuyButtonClick()
    {
        // 인벤토리 참조
        Inventory inventory = Inventory.instance;

        // 인벤토리에 아이템 추가 시도
        bool added = inventory.AddItem(item);

        if (added)
        {
            Debug.Log("Item added to inventory: " + item.itemName);
            // 추가로 아이템을 슬롯에서 제거하거나 상점 재고에서 줄이는 등의 로직 추가 가능
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
