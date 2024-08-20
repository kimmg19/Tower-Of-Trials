using UnityEngine;

public class GolemAttackState : AttackState
{
    private float stompCooldown = 5.0f; // 스톰프 공격 쿨다운 시간
    private float stompTimer = 0.0f;
    private float stompRangeMultiplier = 1.5f; // 스톰프 공격의 범위를 기본 공격보다 넓게 설정

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
            PerformStompAttack(animator);
            stompTimer = 0.0f; // 쿨다운 타이머 초기화
        }
    }

    private void PerformStompAttack(Animator animator)
    {
        // 스톰프 공격 애니메이션 트리거
        animator.SetTrigger("stompAttack");

        // 스톰프 공격의 범위 내에 있는 플레이어에게 데미지 적용
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance <= attackRange * stompRangeMultiplier) // 스톰프 공격 범위는 기본 공격보다 큼
        {
            playerStatus.TakeDamage(25); // 스톰프 공격으로 25의 데미지를 플레이어에게 가함
        }

        Debug.Log("Golem performs a stomp attack.");
    }
}
