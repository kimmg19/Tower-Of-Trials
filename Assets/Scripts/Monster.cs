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
        Debug.Log("ï¿½ï¿½ ï¿½ÈµÇ³ï¿½ ï¿½ï¿½ï¿½ï¿½");
=======
        Debug.Log("¿Ö ¾ÈµÇ³Ä ¤µ¤²");
>>>>>>> 80c62547cadff494c1eb5fa680c3ae517bcc32d0
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
<<<<<<< HEAD
            // ï¿½ï¿½ï¿½Í¿ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
            TakeDamage(10); // ï¿½ï¿½ï¿½Ã·ï¿½ 10ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
            Debug.Log("10 Damage"); // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ß»ï¿½ï¿½ï¿½ ï¿½ï¿½ ï¿½Î±ï¿½ ï¿½ï¿½ï¿½

            
=======
            // ¸ó½ºÅÍ¿¡°Ô µ¥¹ÌÁö¸¦ ÀÔÈû
            TakeDamage(10); // ¿¹½Ã·Î 10ÀÇ µ¥¹ÌÁö¸¦ ÀÔÈû
            Debug.Log("10 Damage"); // µ¥¹ÌÁö ÀÔÈ÷´Â µ¿ÀÛÀÌ ¹ß»ýÇÒ ¶§ ·Î±× Ãâ·Â

            // IsHitted ÆÄ¶ó¹ÌÅÍ¸¦ Æ®¸®°Å·Î È°¼ºÈ­ÇÏ¿© ¾Ö´Ï¸ÞÀÌ¼ÇÀ» Àç»ý
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
        // ¸ó½ºÅÍ°¡ »ç¸ÁÇßÀ» ¶§ÀÇ Ã³¸®
        Debug.Log("Monster died!"); // ¸ó½ºÅÍ°¡ »ç¸ÁÇßÀ» ¶§ ·Î±× Ãâ·Â
>>>>>>> 80c62547cadff494c1eb5fa680c3ae517bcc32d0
        animator.SetTrigger("Die");
        
    }
}
