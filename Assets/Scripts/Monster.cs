using UnityEngine;

public class Monster : MonoBehaviour
{

    public int health;

    private Animator animator;

    void Start()
    {

        Debug.Log("�� �ȵǳ� ����");
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {

            TakeDamage(10); // ���÷� 10�� �������� ����
            Debug.Log("10 Damage"); // ������ ������ ������ �߻��� �� �α� ���

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

       
        Debug.Log("Monster died!"); // ���Ͱ� ������� �� �α� ���

        animator.SetTrigger("Die");
        
    }
}
