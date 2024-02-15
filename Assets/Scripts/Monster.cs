using UnityEngine;

public class Monster : MonoBehaviour
{
    public int health = 100;
    private Animator animator;

    void Start()
    {
        Debug.Log("왜 안되냐 ㅅㅂ");
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            // 몬스터에게 데미지를 입힘
            TakeDamage(10); // 예시로 10의 데미지를 입힘
            Debug.Log("10 Damage"); // 데미지 입히는 동작이 발생할 때 로그 출력

            // IsHitted 파라미터를 트리거로 활성화하여 애니메이션을 재생
            animator.SetTrigger("IsHitted");
        }
    }

    

    void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // 몬스터가 사망했을 때의 처리
        Debug.Log("Monster died!"); // 몬스터가 사망했을 때 로그 출력
        animator.SetTrigger("Die");
        
    }
}
