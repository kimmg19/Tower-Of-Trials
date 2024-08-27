using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Slime : BaseEnemy
{
    private FirstFloorManager firstFloorManager;
    

    // 슬라임의 고유 스탯을 초기화
    protected override void InitializeStats()
    {
        HP = 100; // 슬라임의 체력 설정
        damageAmount = 10; // 슬라임의 공격력 설정
    }

    // 슬라임만의 고유한 죽음 로직
    protected override void Die()
    {
        base.Die();
        // 슬라임의 고유한 죽음 처리를 추가할 수 있음
        if (firstFloorManager != null)
        {
            firstFloorManager.OnSlimeKilled(); // 슬라임이 죽을 때 카운트 증가
        }
    }

    // 시작 시 초기화
    protected override void Start()
    {
        base.Start();
        firstFloorManager = FindObjectOfType<FirstFloorManager>();

        // 슬라임 자식 오브젝트에서 헬스바 슬라이더를 찾기
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