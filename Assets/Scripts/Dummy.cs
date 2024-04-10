using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator=GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage()
    {
        print("Dummy damaged");
        animator.SetTrigger("Damaged");
    }
}
