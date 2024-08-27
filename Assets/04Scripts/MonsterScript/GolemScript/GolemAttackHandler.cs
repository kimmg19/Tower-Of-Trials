using UnityEngine;
using UnityEngine.AI;

public class GolemAttackHandler : MonoBehaviour
{
    private Transform player;
    private PlayerStatus playerStatus;
    private NavMeshAgent agent;
    
    public float attackRange = 3.0f;
    private float stompRangeMultiplier = 1.5f; // 스톰프 공격의 범위를 기본 공격보다 넓게 설정

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerStatus = player.GetComponent<PlayerStatus>();
        agent = GetComponent<NavMeshAgent>();

        if (playerStatus == null)
        {
            Debug.LogError("PlayerStatus component not found on Player.");
        }

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent not found on the Golem.");
        }
    }

    // 애니메이션 이벤트에서 호출될 메서드
    public void OnStompAttack()
    {
        Debug.Log("Golem performs a stomp attack.");

        // 스톰프 공격의 범위 내에 있는 플레이어에게 데미지 적용
        float distance = Vector3.Distance(player.position, agent.transform.position);
        if (distance <= attackRange * stompRangeMultiplier) // 스톰프 공격 범위는 기본 공격보다 넓음
        {
            playerStatus.TakeDamage(25); // 스톰프 공격으로 25의 데미지를 플레이어에게 가함
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
}
