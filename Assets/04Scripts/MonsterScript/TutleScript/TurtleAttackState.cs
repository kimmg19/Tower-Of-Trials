using UnityEngine;

public class TurtleAttackState : AttackState
{
    Turtle turtle;
    protected override void OnStateEnterCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnterCustom(animator, stateInfo, layerIndex);
        // Turtle의 공격 상태 진입 시 추가적인 동작을 정의
        // Debug.Log("Slime is preparing to attack.");
        turtle=animator.GetComponent<Turtle>();
    }

    protected override void OnStateUpdateCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdateCustom(animator, stateInfo, layerIndex);
        // 공격 중에 Turtle만의 행동을 추가
        if (distance > turtle.attackRange || !playerStatus.playerAlive)
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
        // 공격 상태 종료 시 Turtle만의 행동을 추가
    }
}
