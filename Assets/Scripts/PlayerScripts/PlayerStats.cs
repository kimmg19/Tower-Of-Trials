// PlayerStats.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{   
    public float playerSpeed = 5f;
    public float sprintSpeed = 1.5f;
    public int maxHp = 100;
    public int maxStamina = 50;
    [SerializeField] private int _currentHp;
    public int currentHp
    {
        get { return _currentHp; }
        set { _currentHp = value; }
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
        currentStamina = maxStamina;
    }    
}
