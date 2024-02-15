using UnityEngine;

public class Monster : MonoBehaviour
{
    public int health = 100;
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
            // ���Ϳ��� �������� ����
            TakeDamage(10); // ���÷� 10�� �������� ����
            Debug.Log("10 Damage"); // ������ ������ ������ �߻��� �� �α� ���

            // IsHitted �Ķ���͸� Ʈ���ŷ� Ȱ��ȭ�Ͽ� �ִϸ��̼��� ���
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
        // ���Ͱ� ������� ���� ó��
        Debug.Log("Monster died!"); // ���Ͱ� ������� �� �α� ���
        animator.SetTrigger("Die");
        
    }
}
