using UnityEngine;

public class MuscomorphChaseState : ChaseState
{
    Muscomorph muscomorph;
    protected override void OnStateEnterCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnterCustom(animator, stateInfo, layerIndex);
        // Muscomorph의 추격 상태 진입 시 추가적인 동작을 정의
        muscomorph = animator.GetComponent<Muscomorph>();

    }

    protected override void OnStateUpdateCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdateCustom(animator, stateInfo, layerIndex);
        // 추격 중에 Muscomorph만의 행동을 추가
        if (distance <= muscomorph.attackRange && playerStatus.playerAlive)
        {

            animator.SetBool("isAttacking", true);
        }

    }

    protected override void OnStateExitCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExitCustom(animator, stateInfo, layerIndex);
        // 추격 상태 종료 시 Muscomorph만의 행동을 추가
    }
}
