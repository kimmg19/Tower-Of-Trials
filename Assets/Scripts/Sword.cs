using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    Character character;
    Enemy enemy;
    Dummy dummy;
    TrailRenderer trailRenderer;
    private void Start()
    {
        character = FindObjectOfType<Character>();
        enemy = FindObjectOfType<Enemy>();
        dummy = FindObjectOfType<Dummy>();
    }
    
    public int damageAmount = 20;
    private void OnTriggerEnter(Collider other)
    {
        if (character.enableDamaging)
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
