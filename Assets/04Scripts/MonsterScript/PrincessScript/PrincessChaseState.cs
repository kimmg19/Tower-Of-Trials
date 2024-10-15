using System.Collections;
using System.Diagnostics;
using UnityEngine;



public class PrincessChaseState : ChaseState
{
    Princess princess;
    protected override void OnStateEnterCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnterCustom(animator, stateInfo, layerIndex);
        // Golem의 추격 상태 진입 시 추가적인 동작을 정의
        princess = animator.GetComponent<Princess>();

    }

    protected override void OnStateUpdateCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdateCustom(animator, stateInfo, layerIndex);

        

        // 플레이어와의 거리가 일정 이하일 경우 공격 상태로 전환
        if (distance <= princess.attackRange && playerStatus.playerAlive)
        {
            int num = Random.Range(1, 4);
            //Debug.LogError("공격 번호 : "+num);
            animator.SetInteger("AttackNum", num);            
            animator.SetBool("isAttacking", true);
        }
    }
    


    protected override void OnStateExitCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExitCustom(animator, stateInfo, layerIndex);
        // 추격 상태 종료 시 dragon 행동을 추가

    }
}