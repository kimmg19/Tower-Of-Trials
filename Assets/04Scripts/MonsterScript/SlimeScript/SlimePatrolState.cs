using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePatrolState : PatrolState
{
    // 슬라임이 Patrol 상태에 들어올 때 실행되는 커스텀 로직
    protected override void OnStateEnterCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 부모 클래스의 OnStateEnterCustom을 호출하여 기본 순찰 로직을 실행합니다.
        base.OnStateEnterCustom(animator, stateInfo, layerIndex);

        // 슬라임의 특수한 로직을 여기서 추가할 수 있습니다.
        // Debug.Log("Slime is patrolling with specific behavior.");
    }
}
