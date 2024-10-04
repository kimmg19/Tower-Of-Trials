using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackHandler : MonoBehaviour
{
    [SerializeField] private Collider attackCollider; // Inspector에서 할당할 수 있도록 다시 필드로 설정
    private BaseEnemy enemy;

    void Start()
    {
        enemy = GetComponent<BaseEnemy>();

        // Collider가 Inspector에서 할당되지 않은 경우 경고 메시지 출력
        if (attackCollider == null)
        {
            Debug.LogWarning($"{enemy.gameObject.name}에 공격 콜라이더가 할당되지 않았습니다. Inspector에서 할당해주세요.");
        }
    }

    // 애니메이션 이벤트에서 호출할 메서드: 공격이 가능해짐
    void DamageAble()
    {
        // 패링 상태에서는 공격 콜라이더를 활성화하지 않음
        if (enemy.isParried)
        {
            Debug.Log($"{enemy.gameObject.name}은 패링되어 공격할 수 없습니다.");
            return;
        }

        if (enemy.isAttacking)
        {
            enemy.enableDamaging = true;
            if (attackCollider != null)
            {
                attackCollider.enabled = true; // 공격 콜라이더 활성화
                //Debug.Log($"{enemy.gameObject.name}이(가) 이제 플레이어에게 피해를 줄 수 있습니다.");
            }
        }
    }

    // 애니메이션 이벤트에서 호출할 메서드: 공격이 불가능해짐
    void DamageDisable()
    {
        enemy.enableDamaging = false;
        if (attackCollider != null)
        {
            attackCollider.enabled = false; // 공격 콜라이더 비활성화
            //Debug.Log($"{enemy.gameObject.name}이(가) 더 이상 플레이어에게 피해를 줄 수 없습니다.");
        }
    }
}
