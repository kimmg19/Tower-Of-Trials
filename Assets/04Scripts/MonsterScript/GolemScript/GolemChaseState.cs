using UnityEngine;

public class GolemChaseState : ChaseState
{
    
    protected override void OnStateEnterCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnterCustom(animator, stateInfo, layerIndex);
        // Golem의 추격 상태 진입 시 추가적인 동작을 정의
    }

    protected override void OnStateUpdateCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdateCustom(animator, stateInfo, layerIndex);
        Debug.Log("자식");

    }

    protected override void OnStateExitCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExitCustom(animator, stateInfo, layerIndex);
        // 추격 상태 종료 시 Golem만의 행동을 추가
    }
}