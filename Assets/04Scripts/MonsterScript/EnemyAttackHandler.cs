using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EnemyAttackHandler : MonoBehaviour
{
    [SerializeField]SphereCollider sphereCollider;
    BaseEnemy enemy;
    void Start()
    {
        enemy = GetComponent<BaseEnemy>();
        sphereCollider.enabled = false;
    }


    // 애니메이션 이벤트로 호출될 메서드
    void DamageAble()
    {
        enemy.enableDamaging = true;
        if (sphereCollider != null)
        {
            sphereCollider.enabled = true;
        }
    }

    // 애니메이션 이벤트로 호출될 메서드
    void DamageDisable()
    {
        enemy.enableDamaging = false;
        if (sphereCollider != null)
        {
            sphereCollider.enabled = false;
        }
    }
}
