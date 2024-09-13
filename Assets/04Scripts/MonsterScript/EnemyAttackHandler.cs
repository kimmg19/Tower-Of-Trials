using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackHandler : MonoBehaviour
{
    [SerializeField] private Collider attackCollider; // 다양한 콜라이더 타입 지원
    private BaseEnemy enemy;

    void Start()
    {
        enemy = GetComponent<BaseEnemy>();

        // 하이어라키에서 Collider 컴포넌트를 검색하여 자동 할당
        if (attackCollider == null)
        {
            attackCollider = GetComponentInChildren<Collider>(); // 자식 오브젝트에서 Collider 찾기
        }

        // Collider가 없으면 경고 메시지 출력
        if (attackCollider == null)
        {
            Debug.LogWarning($"{enemy.gameObject.name} does not have an attack collider assigned or found in the hierarchy!");
        }
    }

    // 애니메이션 이벤트에서 호출할 메서드: 공격이 가능해짐
    void DamageAble()
    {
        // 패링 상태에서는 공격 콜라이더를 활성화하지 않음
        if (enemy.isParried)
        {
            Debug.Log($"{enemy.gameObject.name} is parried and cannot enable damaging.");
            return;
        }

        if (enemy.isAttacking)
        {
            enemy.enableDamaging = true;
            Debug.Log($"{enemy.gameObject.name} can now damage the player.");
        }
    }

    // 애니메이션 이벤트에서 호출할 메서드: 공격이 불가능해짐
    void DamageDisable()
    {
        enemy.enableDamaging = false;
        Debug.Log($"{enemy.gameObject.name} can no longer damage the player.");
    }
}
