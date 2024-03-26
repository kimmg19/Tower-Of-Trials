using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Character character;
    Enemy enemy;
    Animator animator;
    private void Start()
    {
        character = FindObjectOfType<Character>();
        enemy = FindObjectOfType<Enemy>();
        animator = GetComponentInParent<Animator>();
    }
    public int damageAmount = 20;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster" && character.enableDamaging)
        {
            other.GetComponent<Enemy>().TakeDamage(damageAmount);
        }
    }
}
