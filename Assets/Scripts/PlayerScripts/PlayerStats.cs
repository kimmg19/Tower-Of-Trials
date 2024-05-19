// PlayerStats.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float playerSpeed = 5f;
    public float sprintSpeed = 1.5f;
    public int maxHp = 100;
    public int currentHp { get; set; }
    public int maxStamina = 50;
    public int currentStamina { get; set; }
  
    void Awake()
    {
        currentHp = maxHp;
        currentStamina = maxStamina;
    }
}
