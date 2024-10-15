using System.Net.Security;
using UnityEngine;

public class AttackState : BaseState
{
    protected float attackRange;
    protected BaseEnemy enemy;
    protected float distance;
    protected override void OnStateEnterCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            enemy.isAttacking = true; // 공격 상태 시작
        }
    }

    protected override void OnStateUpdateCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playerStatus.playerAlive)
        {
            LookAtPlayerXZ(animator.transform, player.transform);
        }

        distance=Vector3.Distance(player.position, animator.transform.position);
        
        

        // 공격 로직을 추가할 수 있습니다 (예: 데미지를 주는 메서드 호출 등)
    }

    protected override void OnStateExitCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemy != null)
        {
            enemy.isAttacking = false; // 공격 상태 종료
        }
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
