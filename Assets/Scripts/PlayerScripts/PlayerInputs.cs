using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    PlayerMovement playerMovement;
    GameObject inGameCanvas;
    Animator animator;
    AnimationEvents animationEvents;
    public Vector2 moveInput;
    public Vector3 dodgeVec;
    public bool isRunning = false;
    public bool isDodging = false;
    public bool isGPress = false;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        inGameCanvas = GameObject.Find("InGameCanvas");
        animator = GetComponent<Animator>();
        animationEvents = GetComponent<AnimationEvents>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
            animationEvents.isAttacking = true;
            animator.SetTrigger("Attack");
        }
    }
    void OnRoll()
    {
        if (moveInput.magnitude != 0 && !isDodging && playerMovement.characterController.isGrounded)
        {
            isDodging = true;
            AudioManager.instance.Play("PlayerRoll");
            dodgeVec = playerMovement.CalculateMoveDirection().normalized;
            animator.SetTrigger("Dodge");
            playerMovement.characterController.center = new Vector3(0, 0.5f, 0);
            playerMovement.characterController.height = 1f;
            playerMovement.characterBody.rotation = Quaternion.LookRotation(dodgeVec);
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
