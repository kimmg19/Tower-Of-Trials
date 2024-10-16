using UnityEngine;

public class PrincessIdleState : IdleState
{
    
    protected override void OnStateEnterCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnterCustom(animator, stateInfo, layerIndex);
        // Muscomorph의 대기 상태 진입 시 추가적인 동작을 정의
        // Debug.Log("Slime is idling with specific behavior.");
    }

    protected override void OnStateUpdateCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdateCustom(animator, stateInfo, layerIndex);
       
    }

    protected override void OnStateExitCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExitCustom(animator, stateInfo, layerIndex);
        // 대기 상태 종료 시 Muscomorph만의 행동을 추가
    }
}
