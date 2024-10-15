using Unity.VisualScripting;
using UnityEngine;

public class GolemAttackState : AttackState
{    
    Golem golem;
    protected override void OnStateEnterCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnterCustom(animator, stateInfo, layerIndex);
        golem=animator.GetComponent<Golem>();
    }

    protected override void OnStateUpdateCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdateCustom(animator, stateInfo, layerIndex);

        // 거리가 멀어지면 공격 상태 종료
        if (distance > golem.attackRange || !playerStatus.playerAlive)
        {
            animator.SetBool("isAttacking", false);
            if (enemy != null)
            {
                enemy.isAttacking = false; // 공격 상태 종료
            }
        }
    }
}
