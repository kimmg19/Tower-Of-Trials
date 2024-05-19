using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    Enemy enemy;
    Dummy dummy;
    AnimationEvent animationEvent;
    public int damageAmount = 20;

    private void Start()
    {
        animationEvent =GetComponentInParent<AnimationEvent>();
        enemy = FindObjectOfType<Enemy>();
        dummy = FindObjectOfType<Dummy>();
    }
    void Update()
    {
        if (animationEvent.enableDamaging)
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;
        } else
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (animationEvent.enableDamaging)
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
