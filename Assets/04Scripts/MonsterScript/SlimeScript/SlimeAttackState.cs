using UnityEngine;

public class SlimeAttackState : AttackState
{
    Slime slime;
    protected override void OnStateEnterCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        base.OnStateEnterCustom(animator, stateInfo, layerIndex);
        // 슬라임의 공격 상태 진입 시 추가적인 동작을 정의
        slime=animator.GetComponent<Slime>();
    }

    protected override void OnStateUpdateCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdateCustom(animator, stateInfo, layerIndex);
        // 공격 중에 슬라임만의 행동을 추가
        if (distance > slime.attackRange || !playerStatus.playerAlive)
        {
            animator.SetBool("isAttacking", false);
            if (enemy != null)
            {
                enemy.isAttacking = false; // 공격 상태 종료
            }
        }
    }

    protected override void OnStateExitCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExitCustom(animator, stateInfo, layerIndex);
        // 공격 상태 종료 시 슬라임만의 행동을 추가
    }
}
