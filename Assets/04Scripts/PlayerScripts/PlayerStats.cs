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
        currentHp = maxHp;
        currentMp = maxMp;
        currentStamina = maxStamina;
        Gold = PlayerPrefs.GetInt("PlayerGold", 0); // �⺻���� 0���� ����
    }
    public void IncreaseSwordDamage(int amount)
    {
        if (sword != null)
        {
            sword.damageAmount += amount;
            Debug.Log("Sword damageAmount is now: " + sword.damageAmount);
        }
        else
        {
            Debug.LogWarning("playerSword is null.");
        }
        weaponATK = sword.damageAmount;
    }

    private void Update()
    {
        Attack = upgrade.Attack;
        weaponATK = upgrade.WeaponATK;
        //upgrade.SaveWeaponEnhancePoint();
    }

    public void OnApplicationQuit()
    {
        // ���ø����̼� ���� �� ��� ���� ����
        PlayerPrefs.SetInt("PlayerGold", Gold);
        PlayerPrefs.Save(); // ��� ������ ���� ��� ȣ��
    }
}
