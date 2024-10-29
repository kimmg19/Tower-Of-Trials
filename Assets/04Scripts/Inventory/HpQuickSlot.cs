using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class HpQuickSlot : MonoBehaviour
{
    public Text quantityText; // HP ������ ������ ǥ���� UI Text
    private Inventory inventory;
    [HideInInspector] public bool UsingQuickHpPotion = false;
    [SerializeField] float UsingHpPotionDuration = 1.5f; // ���� ��Ÿ�� ���� �ð�
    [SerializeField] Image UsingHpPotionImage; // ���� ��Ÿ���� ǥ���� Image

    private bool isHpPotionCooldown = false; // ���� ��Ÿ�� ���� ����

    // Input Action Reference
    public InputActionReference useHpPotionAction;

    void Start()
    {
        if (UsingHpPotionImage != null) UsingHpPotionImage.fillAmount = 0;
        inventory = Inventory.instance;
        if (inventory == null)
        {
            return;
        }

        if (useHpPotionAction != null)
        {
            useHpPotionAction.action.performed += UseQuickHpPotion;
        }

        UpdateHpPotionQuantity();
    }

    void OnDestroy()
    {
        if (useHpPotionAction != null)
        {
            useHpPotionAction.action.performed -= UseQuickHpPotion;
        }
    }

    public void UpdateHpPotionQuantity(int quantity)
    {
        quantityText.text = quantity.ToString();
    }

    public void UpdateHpPotionQuantity()
    {
        Item hpPotion = inventory.items.Find(item => item.itemName == "Hp Potion");

        if (hpPotion != null)
        {
            UpdateHpPotionQuantity(hpPotion.quantity);
        }
        else
        {
            UpdateHpPotionQuantity(0);
        }
    }

    public void InvokeUseQuickHpPotion()
    {
        UseQuickHpPotion(new InputAction.CallbackContext());
    }

    private void UseQuickHpPotion(InputAction.CallbackContext context)
    {
        if (isHpPotionCooldown) return; // ��Ÿ�� ���̸� ������ ��� �Ұ�


        Item hpPotion = inventory.items.Find(item => item.itemName == "Hp Potion");

        if (hpPotion != null && hpPotion.quantity > 0)
        {
            PlayerStats playerStats = FindObjectOfType<PlayerStats>();
            if (playerStats == null)
            {
                return;
            }

            bool isUse = hpPotion.Use(playerStats);
            if (isUse)
            {
                hpPotion.quantity -= 1;

                if (hpPotion.quantity == 0)
                {
                    inventory.RemoveItem(inventory.items.IndexOf(hpPotion));
                }

                inventory.SaveInventory();
                UpdateHpPotionQuantity();

                UpdateAllSlotUIs();

                // ��Ÿ�� ����
                StartCoroutine(HpPotionCooldownCoroutine());
            }
 
        }
  
    }

    private IEnumerator HpPotionCooldownCoroutine()
    {
        isHpPotionCooldown = true;
        yield return CooldownCoroutine(UsingHpPotionDuration, UsingHpPotionImage, () => isHpPotionCooldown = false);
    }

    private IEnumerator CooldownCoroutine(float duration, Image cooldownImage, System.Action onComplete)
    {
        float timer = 0f;
        cooldownImage.fillAmount = 1;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            cooldownImage.fillAmount = 1 - (timer / duration);
            yield return null;
        }

        cooldownImage.fillAmount = 0;
        onComplete?.Invoke();
    }

    private void UpdateAllSlotUIs()
    {
        foreach (var slot in FindObjectsOfType<Slot>(true))
        {
            if (slot.item != null && slot.item.itemName == "Hp Potion")
            {
                slot.UpdateSlotUI();
            }
        }
    }
}
