// PlayerStats.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{   
    public float playerSpeed = 4f;
    public float sprintSpeed = 1.5f;
    public float walkSpeed = 0.5f;//천천ㅎ ㅣ 걷기 속도
    public int maxHp = 100;
    public int maxMp = 100;
    public int maxStamina = 50;
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
    }    
}
