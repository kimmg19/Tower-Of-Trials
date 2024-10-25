using UnityEngine;
using UnityEngine.AI;

public class Princess : BaseEnemy
{

    private NavMeshAgent agent;
    [HideInInspector] public float attackRange = 2f;
    FourthAreaManager fourthAreaManager;

    // 골렘의 고유 스탯을 초기화
    protected override void InitializeStats()
    {
        HP = 3000;
        damageAmount = 2;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Die()
    {
        animator.SetTrigger("die");
        AudioManager.instance.Play("MonsterDie");
        GetComponent<Collider>().enabled = false;
        enableDamaging = false;
        fourthAreaManager.OnPrincessKilled();
    }

    protected override void Start()
    {
        base.Start();
        fourthAreaManager = FindObjectOfType<FourthAreaManager>();
        animator.SetBool("isChasing", true);
        agent = GetComponent<NavMeshAgent>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats component not found on Player.");
        }
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent not found on the Princess.");
        }
        if (healthBar == null)
        {
            Debug.LogWarning($"{gameObject.name} is missing a health bar!");
        }
    }
}
