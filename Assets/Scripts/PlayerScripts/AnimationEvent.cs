using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationEvent : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerInputs playerInputs;

    TrailRenderer trailRenderer;
    [HideInInspector]public bool enableDamaging;
    [HideInInspector]public bool isAttacking = false;
    Animator animator;
    void Start()
    {
        playerInputs=GetComponent<PlayerInputs>();
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
        playerInputs.isDodging = false;
        OnFinishAttack();
    }
    public bool IsAttacking()
    {
        return (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") ||
                    animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") ||
                    animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3") || isAttacking);
    }
}


