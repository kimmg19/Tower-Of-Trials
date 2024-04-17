using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float playerSpeed = 5f;
    public float sprintSpeed = 1.5f;
    public int maxHp = 100;
    public int currentHp { get; private set; }
    public int maxMp = 50;
    public int currentMp { get; private set;}
    Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
        currentHp = maxHp;
        currentMp = maxMp;
    }

    public void TakeDamage(int damage) // 데미지 입었을 때, hp 감소
    {
        if (currentHp > 0)
        {
            currentHp -= damage;
            print("플레이어 공격 받음");
            animator.SetTrigger("PlayerHit");
        }        
        else
        {
            print("플레이어 사망");
            Die();
        }
    }
    public void UseMana(int amount) // 마나 사용했을 때, mp감소
    {
        currentMp -= amount;
        if (currentMp < 0)
        {
            currentMp = 0;
        }
    }
    private void Die()
    {
        // 플레이어 사망 처리
        Debug.Log("Player Died");
        // 이후에 애니메이션 추가하면 될듯
    }

    // 체력 회복 등의 추가적인 메서드 구현 가능
}
