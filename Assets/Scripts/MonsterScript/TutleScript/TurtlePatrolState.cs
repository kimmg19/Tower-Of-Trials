using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtlePatrolState : PatrolState
{
    
    protected override void OnStateEnterCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 부모 클래스의 OnStateEnterCustom을 호출하여 기본 순찰 로직을 실행합니다.
        base.OnStateEnterCustom(animator, stateInfo, layerIndex);

        // 특수한 로직을 여기서 추가할 수 있습니다.
        // Debug.Log("Slime is patrolling with specific behavior.");
    }
}
