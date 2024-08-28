using UnityEngine;

public class AttackState : BaseState
{
    protected float attackRange = 2f;

    protected override void OnStateEnterCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 공격 상태 진입 시 특별한 동작 추가 가능
    }

    protected override void OnStateUpdateCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playerStatus.playerAlive)
        {
            LookAtPlayerXZ(animator.transform, player.transform);
        }

        float distance = Vector3.Distance(player.position, animator.transform.position);
        
        // 거리가 멀어지면 공격 상태 종료
        if (distance > attackRange || !playerStatus.playerAlive)
        {
            animator.SetBool("isAttacking", false);
        }

        // 공격 로직을 추가할 수 있습니다 (예: 데미지를 주는 메서드 호출 등)
    }

    protected override void OnStateExitCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 공격 상태 종료 시 특별한 동작 추가 가능
    }
    void LookAtPlayerXZ(Transform animator, Transform player)
    {
        // 플레이어와의 방향 벡터 계산 (Y축 고정)
        Vector3 direction = player.position - animator.position;
        direction.y = 0; // Y축을 0으로 고정하여 수평 방향만 고려

        // 방향 벡터를 기준으로 회전 설정
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            animator.rotation = targetRotation;
        }
    }
}
