using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Muscomorph : BaseEnemy
{
    public GameObject keyPrefab; // 열쇠 아이템 프리팹
    public GameObject particleEffectPrefab; // 파티클 효과 프리팹

    // Muscomorph의 고유 스탯을 초기화
    protected override void InitializeStats()
    {
        HP = 250; // Muscomorph의 체력 설정
        damageAmount = 20; // Muscomorph의 공격력 설정
    }

    // 아이템 드랍 로직 오버라이딩
    protected override void DropItem()
    {
        if (keyPrefab != null)
        {
            // 몬스터의 위치에 아이템 생성
            Vector3 dropPosition = transform.position;
            GameObject item = Instantiate(keyPrefab, dropPosition, Quaternion.identity);

            // 파티클 효과 생성
            if (particleEffectPrefab != null)
            {
                Instantiate(particleEffectPrefab, dropPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning($"{gameObject.name} has no particleEffectPrefab assigned.");
            }

            // LeanTween을 사용해 아이템을 위로 튀어오르게 만듦
            float bounceHeight = 2f; // 아이템이 튀어오를 높이
            float bounceDuration = 0.5f; // 아이템이 튀어오르는 시간
            float fallDuration = 0.5f; // 아이템이 떨어지는 시간

            // 위로 튀어오른 후 떨어지게 설정
            LeanTween.moveY(item, dropPosition.y + bounceHeight, bounceDuration)
                .setEaseOutQuad()
                .setOnComplete(() =>
                    LeanTween.moveY(item, dropPosition.y, fallDuration)
                    .setEaseInOutQuad()); // 여기에 Ease 조정
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} has no keyPrefab assigned.");
        }
    }

    // 시작 시 초기화
    protected override void Start()
    {
        base.Start();
        healthBar = GetComponentInChildren<Slider>();
    }
}
