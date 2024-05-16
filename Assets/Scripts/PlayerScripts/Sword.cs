using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    Enemy enemy;
    Dummy dummy;
    AnimationEvents animationEvents;
    public int damageAmount = 20;

    private void Start()
    {
        animationEvents =GetComponentInParent<AnimationEvents>();
        enemy = FindObjectOfType<Enemy>();
        dummy = FindObjectOfType<Dummy>();
    }
    void Update()
    {
        if (animationEvents.enableDamaging)
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;
        } else
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (animationEvents.enableDamaging)
        {
            if (other.CompareTag("Monster"))
            {
                enemy.TakeDamage(damageAmount);
            } else if (other.CompareTag("Dummy"))
            {
                dummy.TakeDamage();
            } else return;
        }
    }
    
}
