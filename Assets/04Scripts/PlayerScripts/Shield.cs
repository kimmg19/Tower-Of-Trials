using System.Collections;
using System.Threading;
using Unity.XR.Oculus.Input;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] public ParticleSystem parryEffect; // 패링 성공 시 재생할 파티클 시스템
    [SerializeField] public float parryTimingWindow = 1f; // 패링 성공 가능 시간
    [SerializeField] public float damageReductionPercentage = 50f; // 방패 막기 시 데미지 감소 비율
    [SerializeField] public float parryCooldown = 1.0f; // 패링 후 쿨다운 시간
    [SerializeField] public bool isParryWindowActive = false;
    [SerializeField] public bool isBlocking = false;
    [SerializeField] public bool canParry = true; // 패링 가능 상태
    private PlayerStatus playerStatus;
    string enemyTag = "AttackCollider";
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
        // 패링 가능 상태일 때만 패링 창을 열 수 있음
        if (canParry && !isParryWindowActive)
        {
            StartCoroutine(ParryWindowCoroutine());
        }
    }

    public void StartBlocking()
    {
        isBlocking = true;
    }

    public void StopBlocking()
    {
        isBlocking = false;
    }

    private IEnumerator ParryWindowCoroutine()
    {
        isParryWindowActive = true;
        yield return new WaitForSeconds(parryTimingWindow);
        isParryWindowActive = false;
    }

    /*private void OnTriggerEnter(Collider other)
    {
        BaseEnemy enemy = other.GetComponentInParent<BaseEnemy>();
        if (other.CompareTag(enemyTag))
        {
            if (enemy != null)
            {
                if (isParryWindowActive && canParry) // 패링 창과 패링 가능 상태일 때만 패링 성공 처리
                {
                    HandleParrySuccess(enemy);
                } else if (isBlocking)
                {
                    // 방패 막기 중: 데미지 감소 적용
                    int reducedDamage = Mathf.RoundToInt(enemy.GetDamageAmount() * (1 - damageReductionPercentage / 100));
                    playerStatus.TakeDamage(reducedDamage);
                    Debug.Log($"Blocked! Damage reduced to {reducedDamage}.");
                } else
                {
                    // 방패 막기 및 패링 실패 시: 일반 데미지 적용
                    playerStatus.TakeDamage(enemy.GetDamageAmount());
                }
            }
        }
    }*/

    public void HandleParrySuccess(BaseEnemy enemy)
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

        // 패링 후 쿨다운 적용
        StartCoroutine(ResetParryState());
    }

    private IEnumerator ResetParryState()
    {
        canParry = false; // 패링 쿨다운 시작
        isParryWindowActive = false; // 패링 창 비활성화
        playerStatus.SetParrySuccess(false); // 패링 성공 상태 해제

        // 쿨다운 시간 대기
        yield return new WaitForSeconds(parryCooldown);

        canParry = true; // 쿨다운 후 다시 패링 가능
    }

    private void OnTriggerExit(Collider other)
    {
        // 패링 상태 초기화
        if (other.CompareTag(enemyTag))
        {            
            playerStatus.SetParrySuccess(false);
        }
    }
}
