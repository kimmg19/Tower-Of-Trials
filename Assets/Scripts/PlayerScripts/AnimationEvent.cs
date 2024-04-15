using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    PlayerMovement playerMovement;
    TrailRenderer trailRenderer;
    public bool enableDamaging;
    public bool isAttacking = false;
    Animator animator;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();
        trailRenderer.gameObject.SetActive(false);
        animator = GetComponent<Animator>();
    }
    void DamageEnable()
    {
        enableDamaging = !enableDamaging;
    }


    void AttackEffectOn()
    {
        trailRenderer.gameObject.SetActive(true);
    }
    void AtttackEffectOff()
    {
        trailRenderer.gameObject.SetActive(false);
    }
    public void OnFinishAttack()
    {
        isAttacking = false;
    }
    void EndDodge()
    {
        playerMovement.characterController.center = new Vector3(0, 0.88f, 0);
        playerMovement.characterController.height = 1.6f;
        playerMovement.isDodging = false;
        OnFinishAttack();
    }
    public bool IsAttacking()
    {
        return (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") ||
                    animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") ||
                    animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3") || isAttacking);
    }
}


