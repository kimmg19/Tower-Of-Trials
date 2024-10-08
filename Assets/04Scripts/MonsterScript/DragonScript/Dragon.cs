using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Dragon : BaseEnemy
{
    //private FirstAreaManager firstAreaManager;
    private Transform player;
    NavMeshAgent agent;
    public float attackRange = 10f;
    


    // 고유 스탯을 초기화
    protected override void InitializeStats()
    {
        HP = 1000;
        damageAmount = 50;
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

       
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent not found on the dragon.");
        }
        if (healthBar == null)
        {
            Debug.LogWarning($"{gameObject.name} is missing a health bar!");
        }
    }
    

    
}
