using UnityEngine;

public class GolemAttackState : AttackState
{
    public float stompCooldown = 5.0f; // 스톰프 공격 쿨다운 시간
    public float stompTimer = 0.0f;

    protected override void OnStateEnterCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnterCustom(animator, stateInfo, layerIndex);
        stompTimer = 0.0f; // 스톰프 타이머 초기화
    }

    protected override void OnStateUpdateCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdateCustom(animator, stateInfo, layerIndex);

        if (!playerStatus.playerAlive)
        {
            return; // 플레이어가 죽으면 추가 공격하지 않음
        }

        stompTimer += Time.deltaTime;

        if (stompTimer >= stompCooldown)
        {
            animator.SetTrigger("stompAttack"); // 스톰프 공격 애니메이션 트리거
            stompTimer = 0.0f; // 쿨다운 타이머 초기화
        }
    }
}
