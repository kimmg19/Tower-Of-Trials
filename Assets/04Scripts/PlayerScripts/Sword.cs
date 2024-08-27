using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public int damageAmount = 20;
    private MeshCollider swordCollider;
    public PlayerStats playerstats;


    private void Start()
    {
        swordCollider = GetComponent<MeshCollider>();
        if (swordCollider != null)
        {
            swordCollider.enabled = false; // 기본적으로 콜라이더 비활성화
            swordCollider.convex = true; // 물리 연산에 사용할 수 있도록 Convex로 설정
        }
        else
        {
            Debug.LogError("Sword object does not have a MeshCollider component.");
        }
    }

    private void Update()
    {
        damageAmount = playerstats.Attack;
    }

    // 애니메이션 이벤트에서 호출할 메서드
    public void EnableCollider()
    {
        if (swordCollider != null)
        {
            swordCollider.enabled = true;
        }
    }

    // 애니메이션 이벤트에서 호출할 메서드
    public void DisableCollider()
    {
        if (swordCollider != null)
        {
            swordCollider.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (swordCollider.enabled) // 콜라이더가 활성화된 상태에서만 충돌 처리
        {
            BaseEnemy enemy = other.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount); // BaseEnemy에 데미지를 준다.
            }
            else if (other.CompareTag("Dummy"))
            {
                Dummy dummy = other.GetComponent<Dummy>();
                if (dummy != null)
                {
                    dummy.TakeDamage(); // Dummy에 데미지를 준다.
                }
            }
        }
    }
}