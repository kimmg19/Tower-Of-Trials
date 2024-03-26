using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    NavMeshAgent agent; // NavMeshAgent 컴포넌트를 저장하기 위한 변수
    Transform player; // 플레이어의 Transform을 저장하기 위한 변수

    // 상태가 시작될 때 호출되는 메서드
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // NavMeshAgent 컴포넌트를 가져와서 agent 변수에 할당
        agent = animator.GetComponent<NavMeshAgent>();
        // 플레이어의 Transform을 찾아서 player 변수에 할당
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // 캐릭터의 이동 속도를 설정
        agent.speed = 3.5f;
    }

    // 상태가 업데이트될 때 호출되는 메서드
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.LookAt(player);

        // 캐릭터가 플레이어를 향해 이동하도록 설정
        agent.SetDestination(player.position);

        // 캐릭터와 플레이어 사이의 거리를 계산
        float distance = Vector3.Distance(player.position, animator.transform.position);

        // 캐릭터와 플레이어 사이의 거리가 10 이상이면 추격 상태를 종료하도록 설정
        if (distance > 10)
        {
            animator.SetBool("isChasing", false);
        }

        // 캐릭터와 플레이어 사이의 거리가 1이하이면 공격 상태로 전환하도록 설정
        if (distance < 1.8f)
        {
            animator.SetBool("isAttacking", true);
        }
    }

    // 상태가 종료될 때 호출되는 메서드
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 캐릭터의 이동 목적지를 현재 위치로 설정하여 이동을 멈춤
        agent.SetDestination(animator.transform.position);
    }

    // 애니메이터가 움직인 후 호출되는 메서드
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that processes and affects root motion
        // 루트 모션을 처리하고 영향을 미치는 코드 구현
    }

    // 애니메이터의 IK(역무)가 적용된 후 호출되는 메서드
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that sets up animation IK (inverse kinematics)
        // 애니메이션 역무를 설정하는 코드 구현
    }
}
