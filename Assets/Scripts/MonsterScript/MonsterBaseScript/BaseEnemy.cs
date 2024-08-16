using UnityEngine;
using UnityEngine.UI;

public abstract class BaseEnemy : MonoBehaviour
{
    protected int HP;
    protected int damageAmount;
    public Slider healthBar;
    public Animator animator;
    public bool enableDamaging = false;

    protected PlayerStats playerStats;
    protected PlayerStatus playerStatus;

    protected virtual void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();
            playerStatus = player.GetComponent<PlayerStatus>();
        }

        InitializeStats(); // 하위 클래스에서 고유의 값 설정
    }

    protected virtual void Update()
    {
        healthBar.value = HP;
    }

    protected abstract void InitializeStats(); // 파생 클래스에서 값을 초기화하도록 강제

    public virtual void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if (HP <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("damage");
            Debug.Log($"{gameObject.name} got damage");
        }
    }

    protected virtual void Die()
    {
        animator.SetTrigger("die");
        GetComponent<Collider>().enabled = false;
        Invoke("DestroyEnemy", 5f);
    }

    protected virtual void DestroyEnemy()
    {
        gameObject.SetActive(false);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (enableDamaging && other.CompareTag("Player") && playerStatus.playerAlive)
        {
            playerStatus.TakeDamage(damageAmount);
        }
    }
}
