using UnityEngine;

public class Monster : MonoBehaviour
{
<<<<<<< HEAD
    public int health;
=======
    public int health = 100;
>>>>>>> 80c62547cadff494c1eb5fa680c3ae517bcc32d0
    private Animator animator;

    void Start()
    {
<<<<<<< HEAD
        Debug.Log("�� �ȵǳ� ����");
=======
        Debug.Log("�� �ȵǳ� ����");
>>>>>>> 80c62547cadff494c1eb5fa680c3ae517bcc32d0
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
<<<<<<< HEAD
            // ���Ϳ��� �������� ����
            TakeDamage(10); // ���÷� 10�� �������� ����
            Debug.Log("10 Damage"); // ������ ������ ������ �߻��� �� �α� ���

            
=======
            // ���Ϳ��� �������� ����
            TakeDamage(10); // ���÷� 10�� �������� ����
            Debug.Log("10 Damage"); // ������ ������ ������ �߻��� �� �α� ���

            // IsHitted �Ķ���͸� Ʈ���ŷ� Ȱ��ȭ�Ͽ� �ִϸ��̼��� ���
>>>>>>> 80c62547cadff494c1eb5fa680c3ae517bcc32d0
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
<<<<<<< HEAD
        
        Debug.Log("Monster died!"); 
=======
        // ���Ͱ� ������� ���� ó��
        Debug.Log("Monster died!"); // ���Ͱ� ������� �� �α� ���
>>>>>>> 80c62547cadff494c1eb5fa680c3ae517bcc32d0
        animator.SetTrigger("Die");
        
    }
}
