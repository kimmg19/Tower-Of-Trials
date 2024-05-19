using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]GameObject dieImage;
    public float playerSpeed = 5f;
    public float sprintSpeed = 1.5f;
    public int maxHp = 100;
    public int currentHp { get; private set; }
    public int maxMp = 50;
    public int currentMp { get; private set; }
    Animator animator;
    [HideInInspector] public bool playerAlive = true;

    void Start()
    {
        dieImage = GameObject.Find("die");
        dieImage.SetActive(false);
    }
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
            animator.SetTrigger("PlayerHit");
            AudioManager.instance.Play("PlayerHit");
            print("플레이어 공격 받음");
        } else if (currentHp <= 0)
        {
            Die();
        }
    }
    public void UseMana(int amount) // 마나 사용했을 때, mp감소 - 나중에 스태미나 형식으로 바꿀듯
    {
        currentMp -= amount;
        if (currentMp < 0)
        {
            currentMp = 0;
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


    // 체력 회복 등의 추가적인 메서드 구현 가능
}
