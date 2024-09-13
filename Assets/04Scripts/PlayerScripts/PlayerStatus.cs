using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour
{
    private PlayerHitEffect playerHitEffect;
    private PlayerStats playerStats;
    private PlayerInputs playerInputs;
    private Animator animator;
    private InGameCanvas inGameCanvas;

    [HideInInspector] public bool playerAlive = true;
    [HideInInspector] public bool isParried = false;

    // 데미지를 무시하는 상태 관리 변수
   public bool isDamageIgnored = false;
    [SerializeField] private float ignoreDamageDuration = 2.0f; // 무적 시간 설정

    void Start()
    {
        playerHitEffect = GetComponent<PlayerHitEffect>();
        playerInputs = GetComponent<PlayerInputs>();
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        inGameCanvas = FindObjectOfType<InGameCanvas>();
    }

    public void TakeDamage(int damage, bool isParried = false)
    {
        // 무적 상태일 때 데미지를 무시
        if (isDamageIgnored)
        {
            Debug.Log("Damage ignored due to invincibility.");
            return;
        }

        // 패링에 성공한 경우
        if (isParried)
        {
            Debug.Log("Attack was parried. Player should be invincible.");
            SetParrySuccess(true); // 패링 성공 상태 설정
            return;
        }

        // 회피 상태가 아닐 때만 데미지를 받음
        if (!playerInputs.isDodging)
        {
            int finalDamage = damage;

            if (playerStats.currentHp > 0)
            {
                playerHitEffect.ShowHitEffect();
                playerStats.currentHp -= finalDamage;
                AudioManager.instance.Play("PlayerHit");

                Debug.Log("Player took damage.");

                if (playerStats.currentHp <= 0)
                {
                    Die();
                }
            }
        }
    }

    private IEnumerator IgnoreDamageForDuration()
    {
        isDamageIgnored = true;
        Debug.Log("Invincibility activated for " + ignoreDamageDuration + " seconds.");
        yield return new WaitForSeconds(ignoreDamageDuration);
        isDamageIgnored = false;
        Debug.Log("Invincibility ended.");
    }

    public void UseStamina(int amount)
    {
        playerStats.currentStamina = Mathf.Max(playerStats.currentStamina - amount, 0);
    }

    public void UseMp(int amount)
    {
        playerStats.currentMp = Mathf.Max(playerStats.currentMp - amount, 0);
    }

    private void Die()
    {
        if (!playerAlive)
            return;

        animator.SetTrigger("PlayerDie");

        // 모든 BGM을 멈추고 사망 효과음 재생
        AudioManager.instance.StopAllBgm();
        AudioManager.instance.Play("PlayerDie");
        AudioManager.instance.Play("PlayerDieBgm");

        playerAlive = false;
        inGameCanvas.dieImage.SetActive(true);
    }

    // 패링 성공 여부 설정 메서드
    public void SetParrySuccess(bool success)
    {
        isParried = success;
        if (success)
        {
            Debug.Log("Parry was successful. Player is now invincible.");
            StartCoroutine(IgnoreDamageForDuration()); // 패링 후 일정 시간 동안 무적 상태
        }
    }
}
