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
    private float stompRangeMultiplier = 1.5f; // 스톰프 공격의 범위를 기본 공격보다 넓게 설정

    // 골렘의 고유 스탯을 초기화
    protected override void InitializeStats()
    {
        HP = 500; // 골렘의 체력 설정
        damageAmount = 30; // 골렘의 공격력 설정
    }

    // 골렘만의 고유한 죽음 로직
    protected override void Die()
    {
        base.Die();

        // 골렘이 죽었을 때 FirstFloorManager에 알려줌
        if (firstFloorManager != null)
        {
            firstFloorManager.OnGolemKilled();
        }
    }

    // 시작 시 초기화
    protected override void Start()
    {
        base.Start();
        firstFloorManager = FindObjectOfType<FirstFloorManager>();
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        // 골렘 자식 오브젝트에서 헬스바 슬라이더를 찾기
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
        } else
        {
            Debug.Log($"{gameObject.name} has a health bar assigned: {healthBar.name}");
        }
    }

    // 유니티 에디터에서 공격 범위를 시각화
    private void OnDrawGizmosSelected()
    {
        // 기본 공격 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // 스톰프 공격 범위
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange * stompRangeMultiplier);
    }

    // 애니메이션 이벤트에서 호출될 메서드
    public void OnStompAttack()
    {
        Debug.Log("Golem performs a stomp attack.");

        // 스톰프 공격의 범위 내에 있는 플레이어에게 데미지 적용
        float distance = Vector3.Distance(player.position, agent.transform.position);
        if (distance <= attackRange * stompRangeMultiplier) // 스톰프 공격 범위는 기본 공격보다 넓음
        {
            playerStatus.TakeDamage(damageAmount); // 스톰프 공격으로 damageamount 데미지를 플레이어에게 가함
        }
    }
}
