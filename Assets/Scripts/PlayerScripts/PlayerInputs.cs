using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    GameObject inGameCanvas;
    AnimationEvent animationEvent;

    PlayerMovement playerMovement;
    [HideInInspector]public Vector2 moveInput;
    [HideInInspector] public bool isRunning=false;
    [HideInInspector] public bool isDodging;
    [HideInInspector] public bool isGPress;
    Animator animator;
    void Start()
    {
        playerMovement=GetComponent<PlayerMovement>();
        inGameCanvas = GameObject.Find("InGameCanvas");
        animationEvent = GetComponent<AnimationEvent>();
        animator = GetComponent<Animator>();

    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnSprint()
    {
        isRunning = !isRunning;
    }

    void OnAttack()
    {
        if (playerMovement.characterController.isGrounded && !isDodging)
        {
            animationEvent.isAttacking = true;
            animator.SetTrigger("Attack");
        }
    }
    void OnRoll()
    {
        if (moveInput.magnitude != 0 && !isDodging && playerMovement.characterController.isGrounded)
        {
            isDodging = true;
            AudioManager.instance.Play("PlayerRoll");
            playerMovement.dodgeVec = playerMovement.CalculateMoveDirection().normalized;
            animator.SetTrigger("Dodge");
            playerMovement.characterController.center = new Vector3(0, 0.5f, 0);
            playerMovement.characterController.height = 1f;
            playerMovement.characterBody.rotation = Quaternion.LookRotation(playerMovement.dodgeVec);
        }
    }
    void OnInteraction()
    {

        isGPress = true;

    }
    void OnPause()
    {
        inGameCanvas.GetComponent<InGameCanvas>().ClickPuaseButton();
    }
}
