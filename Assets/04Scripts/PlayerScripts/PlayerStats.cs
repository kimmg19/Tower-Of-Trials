// PlayerStats.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Upgrade upgrade;
    public Sword sword;
    public float weaponATK; // 강화까지 포함된 무기공격력
    public int Attack; // 몬스터한테 들어가는 총 공격력
    public float playerSpeed = 4f;
    public float sprintSpeed = 1.5f;
    public float walkSpeed = 0.5f;//천천ㅎ ㅣ 걷기 속도
    public int maxHp = 100;
    public int maxMp = 100;
    public int maxStamina = 50;
    public int Gold = 0;
    public int HpPotionRate = 20; // HP 회복 비율
    public int MpPotionRate = 20; // MP 회복 비율
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
        // 기본값을 설정
        maxStamina = PlayerPrefs.GetInt("PlayerMaxStamina", 50);

        // maxStamina가 최소값 이하일 경우 기본값으로 설정
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
        PlayerPrefs.SetInt("PlayerMpPotionRate", MpPotionRate);
        PlayerPrefs.SetInt("PlayerHpPotionRate", HpPotionRate);
        PlayerPrefs.SetInt("PlayerMaxStamina", maxStamina);
        PlayerPrefs.SetInt("PlayerGold", Gold);

        // 디버깅을 위한 로그 추가
        Debug.Log("Saving PlayerPrefs: ");
        Debug.Log("PlayerMpPotionRate: " + MpPotionRate);
        Debug.Log("PlayerHpPotionRate: " + HpPotionRate);
        Debug.Log("PlayerMaxStamina: " + maxStamina);
        Debug.Log("PlayerGold: " + Gold);

        PlayerPrefs.Save();
    }
}
