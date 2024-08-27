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


    // �ִϸ��̼� �̺�Ʈ�� ȣ��� �޼���
    void DamageAble()
    {
        enemy.enableDamaging = true;
        if (sphereCollider != null)
        {
            sphereCollider.enabled = true;
        }
    }

    // �ִϸ��̼� �̺�Ʈ�� ȣ��� �޼���
    void DamageDisable()
    {
        enemy.enableDamaging = false;
        if (sphereCollider != null)
        {
            sphereCollider.enabled = false;
        }
    }
}
