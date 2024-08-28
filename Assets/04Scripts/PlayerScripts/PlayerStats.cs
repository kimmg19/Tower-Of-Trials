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
        Gold = PlayerPrefs.GetInt("PlayerGold", 0); // 기본값을 0으로 설정
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
        // 애플리케이션 종료 시 골드 값을 저장
        PlayerPrefs.SetInt("PlayerGold", Gold);
        PlayerPrefs.Save(); // 즉시 저장을 원할 경우 호출
    }
}
