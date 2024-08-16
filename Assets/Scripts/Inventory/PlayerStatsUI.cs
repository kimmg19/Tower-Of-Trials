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

    public PlayerStats playerStats;
    public PlayerMovement playerMovement;
    public Sword sword;
    public SwordEft swordEft;

    void Update()
    {
        // PlayerStats Ŭ�������� ���� ���¸� ������ UI Text�� ǥ��
        hpText.text = "HP: " + playerStats.currentHp.ToString();
        mpText.text = "MP: " + playerStats.currentMp.ToString();
        staminaText.text = "Stamina: " + playerStats.currentStamina.ToString();
        speedText.text = "Speed: " + playerMovement.speed.ToString();
        attackText.text = "Attack: " + sword.damageAmount.ToString();
        weaponATK.text = "+" + swordEft.SwordAttackPoint.ToString();
    }
}
