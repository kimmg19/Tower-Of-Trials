using UnityEngine;
using UnityEngine.AI;

public class ChaseState : BaseState
{
    public float chaseRange = 10f;
    public float distance;
    protected override void OnStateEnterCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.speed = 3.0f; // 추격 속도 설정
    }

    protected override void OnStateUpdateCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        animator.transform.LookAt(player); // 플레이어를 바라보도록 회전
        agent.SetDestination(player.position); // 플레이어의 위치로 이동

        distance = Vector3.Distance(player.position, animator.transform.position);

        // 거리가 멀어지면 추격 상태 종료
        if (distance > chaseRange || !playerStatus.playerAlive)
        {
            animator.SetBool("isChasing", false);
        }

        // 플레이어와의 거리가 1.3 이하일 경우 공격 상태로 전환
        if (distance < 2.0f && playerStatus.playerAlive)
        {
            Debug.Log("기본");
            animator.SetBool("isAttacking", true);
        }
    }

    protected override void OnStateExitCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position); // 이동 정지
    }
}
