using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Turtle : BaseEnemy
{
    private FirstFloorManager firstFloorManager;

    // 거북이의 고유 스탯을 초기화
    protected override void InitializeStats()
    {
        HP = 150; // 거북이의 체력 설정
        damageAmount = 15; // 거북이의 공격력 설정
    }

    // 거북이만의 고유한 죽음 로직
    protected override void Die()
    {
        base.Die();
        if (firstFloorManager != null)
        {
            firstFloorManager.OnTurtleKilled(); // 거북이가 죽을 때 카운트 증가
        }
    }

    // 시작 시 초기화
    protected override void Start()
    {
        base.Start();
        firstFloorManager = FindObjectOfType<FirstFloorManager>();

        // 거북이 자식 오브젝트에서 헬스바 슬라이더를 찾기
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
