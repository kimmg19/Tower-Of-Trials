using Unity.VisualScripting;
using UnityEngine;

public class SlimeChaseState : ChaseState
{
    protected override void OnStateEnterCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnterCustom(animator, stateInfo, layerIndex);
        // 슬라임의 추격 상태 진입 시 추가적인 동작을 정의
        // Debug.Log("Slime has started chasing the player.");
    }

    protected override void OnStateUpdateCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdateCustom(animator, stateInfo, layerIndex);


        // 플레이어와의 거리가 일정 이하일 경우 공격 상태로 전환
        if (distance < 1.7f && playerStatus.playerAlive)
        {
           
            animator.SetBool("isAttacking", true);
        }


    }

    protected override void OnStateExitCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExitCustom(animator, stateInfo, layerIndex);
        // 추격 상태 종료 시 슬라임만의 행동을 추가
    }
}
