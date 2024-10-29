using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MpQuickSlot : MonoBehaviour
{
    public Text quantityText; // MP ������ ������ ǥ���� UI Text
    private Inventory inventory;
    [HideInInspector] public bool UsingQuickHpPotion = false;
    [SerializeField] float UsingMpPotionDuration = 1.5f; // ���� ��Ÿ�� ���� �ð�
    [SerializeField] Image UsingMpPotionImage; // ���� ��Ÿ���� ǥ���� Image

    private bool isMpPotionCooldown = false; // ���� ��Ÿ�� ���� ����

    // Input Action Reference
    public InputActionReference useMpPotionAction;

    void Start()
    {
        if (UsingMpPotionImage != null) UsingMpPotionImage.fillAmount = 0;
        inventory = Inventory.instance;
        if (inventory == null)
        {
            return;
        }

        if (useMpPotionAction != null)
        {
            useMpPotionAction.action.performed += UseQuickMpPotion;
        }
 

        UpdateMpPotionQuantity();
    }

    void OnDestroy()
    {
        if (useMpPotionAction != null)
        {
            useMpPotionAction.action.performed -= UseQuickMpPotion;
        }
    }

    public void UpdateMpPotionQuantity(int quantity)
    {
        quantityText.text = quantity.ToString();
    }

    public void UpdateMpPotionQuantity()
    {
        Item mpPotion = inventory.items.Find(item => item.itemName == "Mp Potion");

        if (mpPotion != null)
        {
            UpdateMpPotionQuantity(mpPotion.quantity);
        }
        else
        {
            UpdateMpPotionQuantity(0);
        }
    }

    public void InvokeUseQuickMpPotion()
    {
        UseQuickMpPotion(new InputAction.CallbackContext());
    }

    private void UseQuickMpPotion(InputAction.CallbackContext context)
    {
        if (isMpPotionCooldown) return; 


        Item mpPotion = inventory.items.Find(item => item.itemName == "Mp Potion");

        if (mpPotion != null && mpPotion.quantity > 0)
        {
            PlayerStats playerStats = FindObjectOfType<PlayerStats>();
            if (playerStats == null)
            {
               
                return;
            }

            bool isUse = mpPotion.Use(playerStats);
            if (isUse)
            {
                mpPotion.quantity -= 1;

                if (mpPotion.quantity == 0)
                {
                    inventory.RemoveItem(inventory.items.IndexOf(mpPotion));
                }

                inventory.SaveInventory();
                UpdateMpPotionQuantity();

                UpdateAllSlotUIs();

                // ��Ÿ�� ����
                StartCoroutine(MpPotionCooldownCoroutine());
            }
 
        }
 
    }

    private IEnumerator MpPotionCooldownCoroutine()
    {
        isMpPotionCooldown = true;
        yield return CooldownCoroutine(UsingMpPotionDuration, UsingMpPotionImage, () => isMpPotionCooldown = false);
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
            if (slot.item != null && slot.item.itemName == "Mp Potion")
            {
                slot.UpdateSlotUI();
            }
        }
    }
}
