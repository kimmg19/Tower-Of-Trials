using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    PlayerStats playerstatsss;
    Dummy dummy;
    AnimationEvent animationEvent;
    public int damageAmount;

    private void Start()
    {
        playerstatsss = GetComponent<PlayerStats>();
        animationEvent = GetComponentInParent<AnimationEvent>();
        dummy = FindObjectOfType<Dummy>();
        damageAmount = playerstatsss.Attack;
    }

    void Update()
    {
        // 애니메이션 이벤트에 따라 박스 콜라이더를 활성화하거나 비활성화
        if (animationEvent.enableDamaging)
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;
        } 
        else
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (animationEvent.enableDamaging)
        {
            // 충돌한 오브젝트에서 BaseEnemy 컴포넌트를 찾는다.
            BaseEnemy enemy = other.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount); // 찾은 BaseEnemy에 데미지를 준다.
            }
            else if (other.CompareTag("Dummy"))
            {
                dummy.TakeDamage(); // Dummy에 데미지를 준다.
            }
        }
    }
}
