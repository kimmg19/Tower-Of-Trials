using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Muscomorph : BaseEnemy
{
    private FirstAreaManager firstAreaManager;
    

    // Muscomorph의 고유 스탯을 초기화
    protected override void InitializeStats()
    {
        HP = 250; // Muscomorph의 체력 설정
        damageAmount = 20; // Muscomorph의 공격력 설정
    }

    // Muscomorph만의 고유한 죽음 로직
    protected override void Die()
    {
        base.Die();
        // Muscomorph의 고유한 죽음 처리를 추가할 수 있음

    }

    // 시작 시 초기화
    protected override void Start()
    {
        base.Start();

        // Muscomorph 자식 오브젝트에서 헬스바 슬라이더를 찾기
        healthBar = GetComponentInChildren<Slider>();

    }
}