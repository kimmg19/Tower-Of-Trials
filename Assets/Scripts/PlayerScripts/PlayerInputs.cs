using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    GameObject inGameCanvas;
    AnimationEvent animationEvent;

    PlayerMovement playerMovement;
    PlayerStats playerStats;
    PlayerUI playerUI;
    PlayerStatus playerStatus;  // 추가
    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public bool isRunning = false;
    [HideInInspector] public bool isDodging;
    [HideInInspector] public bool isGPress;
    Animator animator;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playerUI = GetComponent<PlayerUI>();
        playerMovement = GetComponent<PlayerMovement>();
        playerStatus = GetComponent<PlayerStatus>();  // 추가
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
            if (playerStats.currentStamina >= 15) // 스태미나가 충분한지 확인
            {
                playerStatus.UseStamina(15);  // 스태미나 감소
                isDodging = true;
                AudioManager.instance.Play("PlayerRoll");
                playerMovement.dodgeVec = playerMovement.CalculateMoveDirection().normalized;
                animator.SetTrigger("Dodge");
                playerMovement.characterController.center = new Vector3(0, 0.5f, 0);
                playerMovement.characterController.height = 1f;
                playerMovement.characterBody.rotation = Quaternion.LookRotation(playerMovement.dodgeVec);
            }
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
