using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MpQuickSlot : MonoBehaviour
{
    public Text quantityText; // MP 포션의 수량을 표시할 UI Text
    private Inventory inventory;
    [HideInInspector] public bool UsingQuickHpPotion = false;
    [SerializeField] float UsingMpPotionDuration = 1.5f; // 포션 쿨타임 지속 시간
    [SerializeField] Image UsingMpPotionImage; // 포션 쿨타임을 표시할 Image

    private bool isMpPotionCooldown = false; // 포션 쿨타임 상태 변수

    // Input Action Reference
    public InputActionReference useMpPotionAction;

    void Start()
    {
        if (UsingMpPotionImage != null) UsingMpPotionImage.fillAmount = 0;
        inventory = Inventory.instance;
        if (inventory == null)
        {
            Debug.LogError("Inventory instance not found.");
            return;
        }

        if (useMpPotionAction != null)
        {
            useMpPotionAction.action.performed += UseQuickMpPotion;
        }
        else
        {
            Debug.LogError("InputActionReference not assigned.");
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
        if (isMpPotionCooldown) return; // 쿨타임 중이면 아이템 사용 불가

        Debug.Log("UseQuickMpPotion called.");

        Item mpPotion = inventory.items.Find(item => item.itemName == "Mp Potion");

        if (mpPotion != null && mpPotion.quantity > 0)
        {
            PlayerStats playerStats = FindObjectOfType<PlayerStats>();
            if (playerStats == null)
            {
                Debug.LogError("PlayerStats not found.");
                return;
            }

            bool isUse = mpPotion.Use(playerStats);
            if (isUse)
            {
                mpPotion.quantity -= 1;
                Debug.Log("Mp Potion used. Remaining quantity: " + mpPotion.quantity);

                if (mpPotion.quantity == 0)
                {
                    inventory.RemoveItem(inventory.items.IndexOf(mpPotion));
                    Debug.Log("Mp Potion removed from inventory.");
                }

                inventory.SaveInventory();
                UpdateMpPotionQuantity();

                UpdateAllSlotUIs();

                // 쿨타임 시작
                StartCoroutine(MpPotionCooldownCoroutine());
            }
            else
            {
                Debug.LogWarning("Mp Potion use failed.");
            }
        }
        else
        {
            Debug.Log("No Mp Potion found in inventory or quantity is 0.");
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
        Debug.Log("Updating all slot UIs...");
        foreach (var slot in FindObjectsOfType<Slot>(true))
        {
            if (slot.item != null && slot.item.itemName == "Mp Potion")
            {
                slot.UpdateSlotUI();
            }
        }
    }
}
