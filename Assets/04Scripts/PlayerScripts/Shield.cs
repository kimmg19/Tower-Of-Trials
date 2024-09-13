using System.Collections;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private ParticleSystem parryEffect; // 패링 성공 시 재생할 파티클 시스템
    [SerializeField] private float parryTimingWindow = 0.3f; // 패링 성공 가능 시간
    [SerializeField] private float damageReductionPercentage = 50f; // 방패 막기 시 데미지 감소 비율
    private bool isParryWindowActive = false;
    private bool isBlocking = false;
    private PlayerStatus playerStatus;

    private void Start()
    {
        playerStatus = GetComponentInParent<PlayerStatus>();
        Collider collider = GetComponent<Collider>();
        if (collider == null)
        {
            Debug.LogWarning("Shield requires a Collider component.");
        }
        else if (!collider.isTrigger)
        {
            Debug.LogWarning("Shield Collider must be set to 'IsTrigger'.");
        }
    }

    public void ActivateParryWindow()
    {
        if (!isParryWindowActive)
        {
            StartCoroutine(ParryWindowCoroutine());
        }
    }

    public void StartBlocking()
    {
        isBlocking = true;
        // 방패 막기 시작 시 애니메이션 등을 실행할 수 있음
        // GetComponent<Animator>().SetBool("IsBlocking", true);
    }

    public void StopBlocking()
    {
        isBlocking = false;
        // 방패 막기 종료 시 애니메이션 등을 처리
        // GetComponent<Animator>().SetBool("IsBlocking", false);
    }

    private IEnumerator ParryWindowCoroutine()
    {
        isParryWindowActive = true;
        yield return new WaitForSeconds(parryTimingWindow);
        isParryWindowActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            BaseEnemy enemy = other.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                if (isParryWindowActive)
                {
                    HandleParrySuccess(enemy);
                }
                else if (isBlocking)
                {
                    // 방패 막기 중: 데미지 감소 적용
                    int reducedDamage = Mathf.RoundToInt(enemy.GetDamageAmount() * (1 - damageReductionPercentage / 100));
                    playerStatus.TakeDamage(reducedDamage);
                    Debug.Log($"Blocked! Damage reduced to {reducedDamage}.");
                }
                else
                {
                    // 방패 막기 및 패링 실패 시: 일반 데미지 적용
                    playerStatus.TakeDamage(enemy.GetDamageAmount());
                }
            }
        }
    }

    private void HandleParrySuccess(BaseEnemy enemy)
    {
        // 패링 성공 시 처리할 로직
        playerStatus.SetParrySuccess(true);
        enemy.TakeDamage(0, true); // 데미지 0, 패링 상태 true
        Debug.Log("Parry Successful!");

        if (parryEffect != null)
        {
            parryEffect.Play(); // 패링 성공 시 파티클 시스템 재생
        }

        AudioManager.instance.Play("ParrySuccess"); // 패링 성공 사운드 재생

        // 패링 상태 리셋
        StartCoroutine(ResetParryState());
    }

    private IEnumerator ResetParryState()
    {
        yield return new WaitForSeconds(0.5f); // 적절한 시간 이후에 패링 상태 리셋
        playerStatus.SetParrySuccess(false);
        isParryWindowActive = false; // 패링 창 비활성화
    }

    private void OnTriggerExit(Collider other)
    {
        // 패링 상태 초기화
        if (other.CompareTag("Monster"))
        {
            playerStatus.SetParrySuccess(false);
        }
    }
}
