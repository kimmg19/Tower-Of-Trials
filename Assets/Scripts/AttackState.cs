using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    Transform player; // 플레이어의 Transform을 저장하기 위한 변수

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    // 상태가 시작될 때 호출되는 메서드
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 플레이어의 Transform을 찾아서 player 변수에 저장
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    // 상태가 업데이트될 때 호출되는 메서드
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 적 캐릭터가 플레이어를 향해 바라보도록 회전
        animator.transform.LookAt(player);

        // 플레이어와 적 캐릭터의 거리를 계산
        float distance = Vector3.Distance(player.position, animator.transform.position);
        
        // 플레이어와의 거리가 3.5f 이상일 경우 공격 상태를 종료하도록 설정
        if(distance > 3.5f){
            animator.SetBool("isAttacking", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    // 상태가 종료될 때 호출되는 메서드
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 추가적인 동작 없음
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    // 애니메이터가 움직인 후 호출되는 메서드
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that processes and affects root motion
        // 루트 모션을 처리하고 영향을 미치는 코드 구현
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    // 애니메이터의 IK(역무)가 적용된 후 호출되는 메서드
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that sets up animation IK (inverse kinematics)
        // 애니메이션 역무를 설정하는 코드 구현
    }
}
