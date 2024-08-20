using UnityEngine;

public class SlimeIdleState : IdleState
{
    protected override void OnStateEnterCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnterCustom(animator, stateInfo, layerIndex);
        // 슬라임의 대기 상태 진입 시 추가적인 동작을 정의
        // Debug.Log("Slime is idling with specific behavior.");
    }

    protected override void OnStateUpdateCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdateCustom(animator, stateInfo, layerIndex);
        // 대기 중에 슬라임만의 행동을 추가
    }

    protected override void OnStateExitCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExitCustom(animator, stateInfo, layerIndex);
        // 대기 상태 종료 시 슬라임만의 행동을 추가
    }
}
