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

        // 슬라임의 자식 오브젝트에서 슬라이더 컴포넌트를 찾아 할당
        healthBar = GetComponentInChildren<Slider>();

        if (healthBar == null)
        {
            Debug.LogWarning($"{gameObject.name} is missing a health bar!");
        } else
        {
            //Debug.Log($"{gameObject.name} has a health bar assigned: {healthBar.name}");
        }
    }

    protected virtual void Update()
    {

        // 각 슬라임의 헬스바를 개별적으로 업데이트
        if (healthBar != null)
        {
            healthBar.value = HP;
        }
    }

    protected abstract void InitializeStats(); // 파생 클래스에서 값을 초기화하도록 강제

    public virtual void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if (HP <= 0)
        {
            Die();
        } else
        {
            /*if (!animator.GetCurrentAnimatorStateInfo(0).IsName("isAttacking"))
            {
                print("피격 패스");
                return;//isAttacing이 true이면 피격 애니미에션 패스.
            }*/

            animator.SetTrigger("damage");
            Debug.Log($"{gameObject.name} took damage. Current HP: {HP}");
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
            print(other.name+"캐릭터 공격");
            playerStatus.TakeDamage(damageAmount);
        }
    }
}