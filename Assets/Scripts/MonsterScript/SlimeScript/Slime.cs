using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : BaseEnemy
{
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
        Debug.Log("Slime has died!");
    }

    // 시작 시 초기화
    protected override void Start()
    {
        base.Start();
        InitializeStats(); // 슬라임 스탯 초기화
    }
}
