using UnityEngine;

public class IdleState : BaseState
{
    private float timer;
    private float chaseRange = 8f;

    protected override void OnStateEnterCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0; // 타이머 초기화
    }

    protected override void OnStateUpdateCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        // 5초 후 순찰 상태로 전환
        if (timer > 5f)
        {
            animator.SetBool("isPatrolling", true);
        }

        // 플레이어와의 거리 계산 후 추격 상태로 전환
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance < chaseRange && playerStatus.playerAlive)
        {
            animator.SetBool("isChasing", true);
        }
    }

    protected override void OnStateExitCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 특별한 동작 없음
    }
}
