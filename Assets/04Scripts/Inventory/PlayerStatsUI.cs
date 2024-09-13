using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    public Text hpText;
    public Text mpText;
    public Text staminaText;
    public Text speedText;
    public Text attackText;
    public Text weaponATK;
    public Text GoldText;

    public Text HpPotionRateText;
    public Text MpPotionRateText;

    public PlayerStats playerStats;
    public PlayerMovement playerMovement;
    public Sword sword;
    public SwordEft swordEft;
    public Upgrade upgrade;

    void Update()
    {
        // PlayerStats Ŭ�������� ���� ���¸� ������ UI Text�� ǥ��
        hpText.text = "HP: " + playerStats.currentHp.ToString();
        mpText.text = "MP: " + playerStats.currentMp.ToString();
        staminaText.text = "Stamina: " + playerStats.currentStamina.ToString();
        speedText.text = "Speed: " + playerMovement.speed.ToString();
        attackText.text = "Attack: " + upgrade.Attack.ToString();
        weaponATK.text = "+" + upgrade.WeaponATK.ToString();
        GoldText.text = playerStats.Gold.ToString() + " G";
        HpPotionRateText.text = "Hp Potion: " + playerStats.HpPotionRate.ToString() + "%";
        MpPotionRateText.text = "Mp Potion: " + playerStats.MpPotionRate.ToString() + "%";
    }
}
