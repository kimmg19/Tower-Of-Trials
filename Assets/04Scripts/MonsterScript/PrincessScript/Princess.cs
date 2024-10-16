using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Princess : BaseEnemy
{
    
    private NavMeshAgent agent;
    [SerializeField]public float attackRange = 2f;
    public GameObject dragons;
    float time;
    //[SerializeField] private Collider attackCollider; // Attack collider as a serialized field

    // 골렘의 고유 스탯을 초기화
    protected override void InitializeStats()
    {
        HP = 1000;
        damageAmount = 2;
    }
    protected override void Update()
    {        
        base.Update();
        time+=Time.deltaTime;
        if (time > 5f)
        {
            StartCoroutine(DrakarisAttack());
        }
    }

    private IEnumerator DrakarisAttack()
    {
        dragons.SetActive(true);
        animator.SetTrigger("Drakaris");
        yield return new WaitForSeconds(5f);
        dragons.SetActive(false);
        time = 0f;


    }

    protected override void Die()
    {
        base.Die();
    }

    protected override void Start()
    {
        base.Start();
        animator.SetBool("isChasing", true);
        agent = GetComponent<NavMeshAgent>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats component not found on Player.");
        }
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent not found on the Golem.");
        }
        if (healthBar == null)
        {
            Debug.LogWarning($"{gameObject.name} is missing a health bar!");
        }
    }
}
