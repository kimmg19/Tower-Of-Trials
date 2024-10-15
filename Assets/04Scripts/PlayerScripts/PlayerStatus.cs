using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour
{
    private PlayerHitEffect playerHitEffect;
    private PlayerStats playerStats;
    private PlayerInputs playerInputs;
    private Animator animator;
    private InGameCanvas inGameCanvas;
    [SerializeField] public CharacterController characterController;

    private SavePoint savePoint;

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
        characterController = GetComponent<CharacterController>();


        savePoint = FindObjectOfType<SavePoint>();
        if (savePoint == null)
        {
            Debug.LogError("SavePoint를 찾을 수 없습니다.");
        }
    }

    public void TakeDamage(int damage, bool isParried = false)
    {
        // 무적 상태일 때 데미지를 무시
        if (isDamageIgnored)
        {
            Debug.Log("무적 상태로 인해 데미지를 무시합니다.");
            return;
        }

        // 패링에 성공한 경우
        if (isParried)
        {
            Debug.Log("공격이 패링되었습니다. 플레이어는 무적 상태입니다.");
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

                Debug.Log("플레이어가 데미지를 입었습니다.");

                if (playerStats.currentHp <= 0)
                {
                    Die();
                }
            }
        }
    }

    public void UseStamina(int amount)
    {
        playerStats.currentStamina = Mathf.Max(playerStats.currentStamina - amount, 0);
    }

    public void UseMp(int amount)
    {
        playerStats.currentMp = Mathf.Max(playerStats.currentMp - amount, 0);
    }

    public void Die()
    {
        if (!playerAlive)
            return;

        animator.SetTrigger("PlayerDie");

        // 모든 BGM을 멈추고 사망 효과음 재생
        AudioManager.instance.StopAllBgm();
        AudioManager.instance.Play("PlayerDie");
        AudioManager.instance.Play("PlayerDieBgm");

        playerAlive = false;

        // **입력 차단**
        playerInputs.enabled = false;  // 플레이어 입력 비활성화

        // **콜라이더 비활성화**
        characterController.enabled = false;  // 캐릭터 컨트롤러 비활성화
        inGameCanvas.dieImage.SetActive(true);
    }

    public void Descent()
    {
        if (playerStats.currentHp <= 40)
        {
            Die();
            //playerAlive = false;
        }

        playerAlive = false;
        playerStats.currentHp -= 40;
        playerHitEffect.ShowHitEffect();

        if (savePoint != null)
        {
            //animator.SetTrigger("PlayerDie");
            savePoint.LoadPlayerPosition();
            Debug.Log("세이브 포인트로 이동!");
            StartCoroutine(DeactivateAfterDelay());
        }
        else
        {
            Debug.LogWarning("SavePoint가 null입니다.");
        }

        //playerStats.OnApplicationQuit();
    }

    private IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(0.1f); // 1초 대기
        playerAlive = true; // 오브젝트 비활성화
    }

    // 패링 성공 여부 설정 메서드
    public void SetParrySuccess(bool success)
    {
        isParried = success;
        if (success)
        {
            Debug.Log("플레이어는 이제 무적 상태입니다.");
            StartCoroutine(IgnoreDamageForDuration()); // 패링 후 일정 시간 동안 무적 상태
        }
    }
    private IEnumerator IgnoreDamageForDuration()
    {
        isDamageIgnored = true;
        Debug.Log("무적 상태가 " + ignoreDamageDuration + "초 동안 활성화됩니다.");
        yield return new WaitForSeconds(ignoreDamageDuration);
        isDamageIgnored = false;
        Debug.Log("무적 상태가 종료되었습니다.");
    }
}
