using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int HP = 100;
    public Slider healthBar;
    public Animator animator;

    void Update(){
        healthBar.value = HP;
    }
    // damageAmount 만큼 체력을 감소시키고, 체력이 0 이하일 때 애니메이션을 재생합니다.
    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if (HP <= 0)
        {
            // 죽음 애니메이션 재생
            AudioManager.instance.Play("ZombieDie");
            animator.SetTrigger("die");
            GetComponent<Collider>().enabled = false;

            Invoke("DestroyEnemy",5f);
        } else
        {
            // 피격 애니메이션 재생
            AudioManager.instance.Play("ZombieHit");
            animator.SetTrigger("damage");
            Debug.Log("Zombie get damage");
        }
        
    }

    private void DestroyEnemy()
    {
        gameObject.SetActive(false);
    }
    
}
