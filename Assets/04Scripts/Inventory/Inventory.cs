using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    #endregion


    public delegate void OnSlotCountChange(int val);
    public OnSlotCountChange onSlotCountChange;

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    public List<Item> items = new List<Item>();

    private int slotCnt;
    public int SlotCnt
    {
        get => slotCnt;
        set
        {
            slotCnt = value;
            onSlotCountChange?.Invoke(slotCnt);
            SaveSlotCount();
        }
    }

    public HpQuickSlot hpQuickSlot;
    public MpQuickSlot mpQuickSlot;

    void Start()
    {
        hpQuickSlot = FindObjectOfType<HpQuickSlot>(); // HpQuickSlot 인스턴스 찾기
        mpQuickSlot = FindObjectOfType<MpQuickSlot>(); // MpQuickSlot 인스턴스 찾기
        LoadSlotCount(); // Load the slot count from PlayerPrefs
        LoadInventory();
    }

    public bool AddItem(Item _item)
    {
        foreach (var item in items)
        {
            if (item.itemName == _item.itemName) // 동일한 아이템인지 확인
            {
                item.quantity += _item.quantity; // 동일한 아이템일 경우 수량 증가
                Debug.Log("Item added. New quantity: " + item.quantity); // 디버깅 로그 추가
                onChangeItem?.Invoke();
                SaveInventory();

                return true;
            }
        }

        if (items.Count < SlotCnt)
        {
            items.Add(_item);
            Debug.Log("New item added. Quantity: " + _item.quantity); // 디버깅 로그 추가
            onChangeItem?.Invoke();
            SaveInventory();

            return true;
        }

        return false;
    }

    public void RemoveItem(int _index)
    {
        if (_index >= 0 && _index < items.Count)
        {
            Item itemToRemove = items[_index];
            itemToRemove.quantity--; // 수량 감소

            if (itemToRemove.quantity <= 0)
            {
                items.RemoveAt(_index); // 수량이 0 이하가 되면 아이템 제거
                Debug.Log($"Item '{itemToRemove.itemName}' removed from inventory.");
            }
            else
            {
                Debug.Log($"Item '{itemToRemove.itemName}' quantity decreased. New quantity: {itemToRemove.quantity}");
            }

            onChangeItem?.Invoke();
            SaveInventory();
        }
        else
        {
            Debug.LogError($"Index {_index} out of range. Total items: {items.Count}");
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("FieldItem"))
        {
            FieldItems fieldItems = collision.GetComponent<FieldItems>();
            if (AddItem(fieldItems.GetItem()))
                fieldItems.DestroyItem();
        }
        else
        {
            print(collision.name);
        }
    }

    public void SaveInventory()
    {
        for (int i = 0; i < items.Count; i++)
        {
            string itemJson = items[i].ToJson();
            PlayerPrefs.SetString("InventorySlot" + i, itemJson);
        }
        PlayerPrefs.SetInt("InventoryItemCount", items.Count);
        PlayerPrefs.Save();
        Debug.Log("Inventory saved with " + items.Count + " items.");

        if (hpQuickSlot != null)
        {
            // Find Hp Potion and update quantity
            Item hpPotion = items.Find(item => item.itemName == "Hp Potion");
            if (hpPotion != null)
            {
                hpQuickSlot.UpdateHpPotionQuantity(hpPotion.quantity);
            }
            else
            {
                hpQuickSlot.UpdateHpPotionQuantity(0);
            }
        }

        if (mpQuickSlot != null)
        {
            // Find Mp Potion and update quantity
            Item mpPotion = items.Find(item => item.itemName == "Mp Potion");
            if (mpPotion != null)
            {
                mpQuickSlot.UpdateMpPotionQuantity(mpPotion.quantity);
            }
            else
            {
                mpQuickSlot.UpdateMpPotionQuantity(0);
            }
        }
    }

    // 인벤토리 불러오기 메서드
    public void LoadInventory()
    {
        items.Clear();
        int itemCount = PlayerPrefs.GetInt("InventoryItemCount", 0);

        for (int i = 0; i < itemCount; i++)
        {
            string itemJson = PlayerPrefs.GetString("InventorySlot" + i, string.Empty);
            if (!string.IsNullOrEmpty(itemJson))
            {
                Item item = Item.FromJson(itemJson);
                items.Add(item);
            }
        }
        if (onChangeItem != null)
            onChangeItem.Invoke();

        Debug.Log("Inventory loaded with " + items.Count + " items.");

        if (hpQuickSlot != null)
        {
            // Find Hp Potion and update quantity
            Item hpPotion = items.Find(item => item.itemName == "Hp Potion");
            if (hpPotion != null)
            {
                hpQuickSlot.UpdateHpPotionQuantity(hpPotion.quantity);
            }
            else
            {
                hpQuickSlot.UpdateHpPotionQuantity(0);
            }
        }

        if (mpQuickSlot != null)
        {
            // Find Mp Potion and update quantity
            Item mpPotion = items.Find(item => item.itemName == "Mp Potion");
            if (mpPotion != null)
            {
                mpQuickSlot.UpdateMpPotionQuantity(mpPotion.quantity);
            }
            else
            {
                mpQuickSlot.UpdateMpPotionQuantity(0);
            }
        }
    }
    private void SaveSlotCount()
    {
        PlayerPrefs.SetInt("InventorySlotCount", SlotCnt);
        PlayerPrefs.Save();
    }

    private void LoadSlotCount()
    {
        SlotCnt = PlayerPrefs.GetInt("InventorySlotCount", 4); // Default to 4 if no value is found
    }

    public void ResetSlot()
    {
        items.Clear(); // 아이템 리스트 초기화
        onChangeItem?.Invoke(); // UI 갱신을 위해 델리게이트 호출
        SaveInventory(); // 변경된 인벤토리 상태 저장
        Debug.Log("Inventory has been reset. Current item count: " + items.Count);
    }

}
