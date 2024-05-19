// PlayerStatus.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private GameObject dieImage;
    private Animator animator;
    [HideInInspector] public bool playerAlive = true;
    PlayerStats playerStats;

    void Start (){
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
    }

    public void TakeDamage(int damage)
    {
        if (playerStats.currentHp > 0)
        {
            playerStats.currentHp -= damage;
            animator.SetTrigger("PlayerHit");
            AudioManager.instance.Play("PlayerHit");
            print("플레이어 공격 받음");
        } 
        else if (playerStats.currentHp <= 0)
        {
            Die();
        }
    }

    public void UseStamina(int amount)
    {
        playerStats.currentStamina -= amount;
        if (playerStats.currentStamina < 0)
        {
            playerStats.currentStamina = 0;
        }
    }

    private void Die()
    {
        if (!playerAlive)
            return;

        print("플레이어 사망");
        animator.SetTrigger("PlayerDie");
        AudioManager.instance.Play("PlayerDie");
        playerAlive = false;
        dieImage.SetActive(true);
    }
}
