using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Character character;

    private void Start()
        {
        character = GameObject.FindObjectOfType<Character>();        }
    public int damageAmount = 20;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster" && character.isAttacking)
        {
            other.GetComponent<Enemy>().TakeDamage(damageAmount);
        }
    }

}
