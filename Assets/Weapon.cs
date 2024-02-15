using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Animator animator;
    TrailRenderer trailRenderer;
    void Start()
    {
        animator = GetComponentInParent<Animator>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();
    }
    void Update()
    {
        AttackEffect();
    }
    void AttackEffect()
    {
        if ((animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3"))&&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.6f)
        {
            
            trailRenderer.gameObject.SetActive(true);
        } else
        {
            trailRenderer.gameObject.SetActive(false);
        }
    }
}
