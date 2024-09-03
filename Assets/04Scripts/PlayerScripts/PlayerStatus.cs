// PlayerStatus.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    PlayerHitEffect playerHitEffect;
    InGameCanvas inGameCanvas;
    PlayerInputs playerInputs;
    private Animator animator;
    AnimationEvent animationEvent;
    [HideInInspector] public bool playerAlive = true;
    PlayerStats playerStats;

    void Start()
    {
        playerHitEffect = GetComponent<PlayerHitEffect>();
        playerInputs = GetComponent<PlayerInputs>();
        animationEvent = GetComponent<AnimationEvent>();
        inGameCanvas = FindObjectOfType<InGameCanvas>();
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
    }

    public void TakeDamage(int damage)
    {
        if (!playerInputs.isDodging)
        {
            animationEvent.OnFinishAttack();
            animationEvent.AtttackEffectOff();
            if (playerStats.currentHp > 0)
            {
                playerHitEffect.ShowHitEffect();
                playerStats.currentHp -= damage;
                //animator.SetTrigger("PlayerHit");
                AudioManager.instance.Play("PlayerHit");
                print("플레이어 공격 받음");
                if (playerStats.currentHp <= 0)
                {
                    Die();
                }
            }
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
    public void UseMp(int amount)
    {
        playerStats.currentMp -= amount;
        if (playerStats.currentMp < 0)
        {
            playerStats.currentMp = 0;
        }
    }

    private void Die()
    {
        if (!playerAlive)
            return;

        animator.SetTrigger("PlayerDie");

        // 모든 BGM을 멈춤
        AudioManager.instance.StopAllBgm();

        // 사망 효과음을 재생
        AudioManager.instance.Play("PlayerDie");
        AudioManager.instance.Play("PlayerDieSFX");

        playerAlive = false;
        inGameCanvas.dieImage.SetActive(true);
    }

}
