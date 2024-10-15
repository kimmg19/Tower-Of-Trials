using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Princess : BaseEnemy
{
    private Transform player;
    private NavMeshAgent agent;
    public float attackRange = 1.2f;
    
    //[SerializeField] private Collider attackCollider; // Attack collider as a serialized field

    // 골렘의 고유 스탯을 초기화
    protected override void InitializeStats()
    {
        HP = 1000;
        damageAmount = 2;
    }

    protected override void Die()
    {
        base.Die();        
    }

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindWithTag("Player").transform;
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
