using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Golem : BaseEnemy
{
    private FirstFloorManager firstFloorManager;
    private Transform player;
    private NavMeshAgent agent;
    public float attackRange = 3.0f;
    private float stompRangeMultiplier = 1.5f;
    private bool isJumping = false;

    public ParticleSystem rockDebrisEffect;

    // 골렘의 고유 스탯을 초기화
    protected override void InitializeStats()
    {
        HP = 500;
        damageAmount = 30;
    }

    protected override void Die()
    {
        base.Die();

        if (firstFloorManager != null)
        {
            firstFloorManager.OnGolemKilled();
        }
    }

    protected override void Start()
    {
        base.Start();
        firstFloorManager = FindObjectOfType<FirstFloorManager>();
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        healthBar = GetComponentInChildren<Slider>();

        if (playerStats == null)
        {
            Debug.LogError("PlayerStats component not found on Player.");
        }

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent not found on the Golem.");
        }

        if (healthBar == null)
        {
            Debug.LogWarning($"{gameObject.name} is missing a health bar!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange * stompRangeMultiplier);
    }

    public void OnNormalAttack()
    {
        if (rockDebrisEffect != null)
        {
            rockDebrisEffect.Play();
        }
        Debug.Log("Golem performs a stomp attack.");

        float distance = Vector3.Distance(player.position, agent.transform.position);
        if (distance <= attackRange * stompRangeMultiplier)
        {
            playerStatus.TakeDamage(damageAmount);
        }
    }

    // 점프 공격 메서드
    public void JumpAttack(Animator animator)
    {
        if (!isJumping)
        {
            animator.SetTrigger("jumpAttack");
            StartCoroutine(StartJumpAttack(animator));
        }
    }

    IEnumerator StartJumpAttack(Animator animator)
    {
        isJumping = true;

        // 점프 애니메이션 진행 시간 동안 NavMeshAgent의 이동을 멈춤
        agent.isStopped = true;

        Vector3 startPosition = agent.transform.position;
        Vector3 targetPosition = player.position;

        float jumpDuration = 1.0f;
        float elapsedTime = 0f;

        // 점프 높이 및 경로 계산
        while (elapsedTime < jumpDuration)
        {
            elapsedTime += Time.deltaTime;

            float percentComplete = elapsedTime / jumpDuration;

            // 곡선을 따라 이동하면서 목표 위치로 이동
            agent.transform.position = Vector3.Lerp(startPosition, targetPosition, percentComplete);

            yield return null;
        }

        // 점프 후 NavMeshAgent의 위치를 정확히 목표 위치로 설정
        agent.Warp(targetPosition);

        // 착지 후 공격 처리
        float distance = Vector3.Distance(player.position, agent.transform.position);
        if (distance <= attackRange*2)
        {
            if (rockDebrisEffect != null)
            {
                rockDebrisEffect.Play();
            }
            playerStatus.TakeDamage(damageAmount*2);
        }

        // NavMeshAgent 이동 재개
        agent.isStopped = false;
        isJumping = false;
    }
}
