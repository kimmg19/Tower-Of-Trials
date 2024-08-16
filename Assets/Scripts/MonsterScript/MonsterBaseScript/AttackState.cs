using UnityEngine;

public class AttackState : BaseState
{
    private float attackRange = 1.3f;

    protected override void OnStateEnterCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 공격 상태 진입 시 특별한 동작 추가 가능
    }

    protected override void OnStateUpdateCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playerStatus.playerAlive)
        {
            animator.transform.LookAt(player); // 플레이어를 바라보도록 회전
        }

        float distance = Vector3.Distance(player.position, animator.transform.position);

        // 거리가 멀어지면 공격 상태 종료
        if (distance > attackRange || !playerStatus.playerAlive)
        {
            animator.SetBool("isAttacking", false);
        }

        // 공격 로직을 추가할 수 있습니다 (예: 데미지를 주는 메서드 호출 등)
    }

    protected override void OnStateExitCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 공격 상태 종료 시 특별한 동작 추가 가능
    }
}
