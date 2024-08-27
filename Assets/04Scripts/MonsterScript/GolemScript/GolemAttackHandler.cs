using UnityEngine;
using UnityEngine.AI;

public class GolemAttackHandler : MonoBehaviour
{
    private Transform player;
    private PlayerStatus playerStatus;
    private NavMeshAgent agent;
    
    public float attackRange = 3.0f;
    private float stompRangeMultiplier = 1.5f; // ������ ������ ������ �⺻ ���ݺ��� �а� ����

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

    // �ִϸ��̼� �̺�Ʈ���� ȣ��� �޼���
    public void OnStompAttack()
    {
        Debug.Log("Golem performs a stomp attack.");

        // ������ ������ ���� ���� �ִ� �÷��̾�� ������ ����
        float distance = Vector3.Distance(player.position, agent.transform.position);
        if (distance <= attackRange * stompRangeMultiplier) // ������ ���� ������ �⺻ ���ݺ��� ����
        {
            playerStatus.TakeDamage(25); // ������ �������� 25�� �������� �÷��̾�� ����
        }
    }

    // ����Ƽ �����Ϳ��� ���� ������ �ð�ȭ
    private void OnDrawGizmosSelected()
    {
        // �⺻ ���� ����
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // ������ ���� ����
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange * stompRangeMultiplier);
    }
}
