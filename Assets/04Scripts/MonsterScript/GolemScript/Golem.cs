using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Golem : BaseEnemy
{
    private FirstFloorManager firstFloorManager;

    // 골렘의 고유 스탯을 초기화
    protected override void InitializeStats()
    {
        HP = 500; // 골렘의 체력 설정
        damageAmount = 30; // 골렘의 공격력 설정
    }

    // 골렘만의 고유한 죽음 로직
    protected override void Die()
    {
        base.Die();
        
        // 골렘이 죽었을 때 FirstFloorManager에 알려줌
        if (firstFloorManager != null)
        {
            firstFloorManager.OnGolemKilled();
        }
    }

    // 시작 시 초기화
    protected override void Start()
    {
        base.Start();
        firstFloorManager = FindObjectOfType<FirstFloorManager>();

        // 골렘 자식 오브젝트에서 헬스바 슬라이더를 찾기
        healthBar = GetComponentInChildren<Slider>();

        if (healthBar == null)
        {
            Debug.LogWarning($"{gameObject.name} is missing a health bar!");
        }
        else
        {
            //Debug.Log($"{gameObject.name} has a health bar assigned: {healthBar.name}");
        }
    }
}
