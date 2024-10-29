// PlayerStats.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Upgrade upgrade;
    public Sword sword;
    public float weaponATK; // ��ȭ���� ���Ե� ������ݷ�
    public int Attack; // �������� ���� �� ���ݷ�
    public float playerSpeed = 4f;
    public float sprintSpeed = 1.5f;
    public float walkSpeed = 0.5f;//õõ�� �� �ȱ� �ӵ�
    public int maxHp = 100;
    public int maxMp = 100;
    public int maxStamina = 50;
    public int Gold = 0;
    public int HpPotionRate = 20; // HP ȸ�� ����
    public int MpPotionRate = 20; // MP ȸ�� ����
    [SerializeField] private int _currentHp;
    public int currentHp
    {
        get { return _currentHp; }
        set { _currentHp = value; }
    }
    [SerializeField] private int _currentMp;
    public int currentMp
    {
        get { return _currentMp; }
        set { _currentMp = value; }
    }
    [SerializeField] private int _currentStamina;    
    public int currentStamina
    {
        get { return _currentStamina; }
        set { _currentStamina = value; }
    }

    void Awake()
    {
        // �⺻���� ����
        maxStamina = PlayerPrefs.GetInt("PlayerMaxStamina", 50);

        // maxStamina�� �ּҰ� ������ ��� �⺻������ ����
        if (maxStamina < 50)
        {
            maxStamina = 50;
            PlayerPrefs.SetInt("PlayerMaxStamina", maxStamina);
        }

        currentHp = PlayerPrefs.GetInt("PlayerCurrentHp", maxHp);
        currentMp = PlayerPrefs.GetInt("PlayerCurrentMp", maxMp);
        currentStamina = PlayerPrefs.GetInt("PlayerCurrentStamina", maxStamina);
        Gold = PlayerPrefs.GetInt("PlayerGold", 0);
        MpPotionRate = PlayerPrefs.GetInt("PlayerMpPotionRate", 20);
        HpPotionRate = PlayerPrefs.GetInt("PlayerHpPotionRate", 20);
    }

    public void IncreaseSwordDamage(int amount)
    {
        if (sword != null)
        {
            sword.damageAmount += amount;
        }
        
        weaponATK = sword.damageAmount;
    }

    void Update()
    {
        Attack = upgrade.Attack;
        weaponATK = upgrade.WeaponATK;
    }

    public void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("PlayerMpPotionRate", MpPotionRate);
        PlayerPrefs.SetInt("PlayerHpPotionRate", HpPotionRate);
        PlayerPrefs.SetInt("PlayerMaxStamina", maxStamina);
        PlayerPrefs.SetInt("PlayerGold", Gold);

   

        PlayerPrefs.Save();
    }
}
