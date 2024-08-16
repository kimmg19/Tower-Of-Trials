using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    GameObject inGameCanvas;
    AnimationEvent animationEvent;
    LockOnSystem lockOnSystem;  // LockOnSystem 스크립트 참조 추가
    PlayerMovement playerMovement;
    PlayerStats playerStats;
    PlayerUI playerUI;
    PlayerStatus playerStatus;
    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public bool isRunning = false;
    [HideInInspector] public bool isDodging;
    [HideInInspector] public bool isGPress;
    [HideInInspector] public bool isInteracting = false;
    [HideInInspector] public bool isBlocking = false;
    [HideInInspector] public bool isWalking = false;
    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool isJumping = false;
    Animator animator;

    void Start()
    {
        lockOnSystem = GetComponent<LockOnSystem>();  // LockOnSystem 스크립트 초기화
        playerStats = GetComponent<PlayerStats>();
        playerUI = GetComponent<PlayerUI>();
        playerMovement = GetComponent<PlayerMovement>();
        playerStatus = GetComponent<PlayerStatus>();
        inGameCanvas = GameObject.Find("InGameCanvas");
        animationEvent = GetComponent<AnimationEvent>();
        animator = GetComponent<Animator>();
    }

    void OnMove(InputValue value)
    {
        if (isInteracting) return;  // 상호작용 중일 때 입력 무시
        moveInput = value.Get<Vector2>();
    }

    IEnumerator SprintCoroutine()
    {
        while (isRunning && playerStats.currentStamina > 0)
        {
            playerStatus.UseStamina(1);  // 매 프레임마다 스태미너 1씩 감소
            yield return new WaitForSeconds(1.0f);  // 1초 간격으로 처리
        }
    }

    void OnSprint(InputValue value)
    {
        if (isInteracting) return;  // 상호작용 중일 때는 입력 무시

        isRunning = value.isPressed;

        if (isRunning && playerStats.currentStamina > 0)
        {
            StartCoroutine(SprintCoroutine());
        } else
        {
            StopCoroutine(SprintCoroutine());
        }
    }

    void OnWalk(InputValue value)
    {
        if (isInteracting) return;
        isWalking = value.isPressed;
    }

    void OnAttack()
    {
        if (isInteracting || isBlocking) return;  // 상호작용 중일 때는 입력 무시
        if (playerMovement.characterController.isGrounded && !isDodging)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
        }
    }

    void OnRoll()
    {
        if (isInteracting|| moveInput.magnitude == 0|| isDodging|| playerStats.currentStamina < 15) return;  // 상호작용 중일 때는 입력 무시
        playerMovement.Roll();
        isDodging = true;
    }

    void OnInteraction()
    {
        StartCoroutine("Interactting");
        Debug.Log("G key pressed in PlayerInputs.");
    }

    IEnumerator Interactting()
    {
        isGPress = true;
        yield return new WaitForSeconds(1f);
        isGPress = false;
    }

    void OnPause()
    {
        if (isInteracting) return;  // 상호작용 중일 때는 입력 무시
        inGameCanvas.GetComponent<InGameCanvas>().ClickPauseButton();
    }

    // 방어
    void OnBlock(InputValue value)
    {
        if (isInteracting || animationEvent.IsAttacking()) return;  // 상호작용 중일 때는 입력 무시
        animationEvent.OnFinishAttack();
        animationEvent.AtttackEffectOff();
        isBlocking = value.isPressed;
        animator.SetBool("Block", isBlocking);
    }

    // 패링
    void OnParry()
    {
        // 패링 로직 추가 (미구현 상태)
    }

    void OnJump()
    {
        if (!isInteracting && playerMovement.characterController.isGrounded && !isJumping && !animationEvent.IsAttacking())
        {
            // 점프 시작
            StartCoroutine(JumpCoroutine());
            playerMovement.Jump(); // 점프 메서드 호출
        }
    }

    IEnumerator JumpCoroutine()
    {
        isJumping = true;
        yield return new WaitForSeconds(2f);
        isJumping = false;
    }

    void OnLockOn()
    {
        if (isInteracting) return;  // 상호작용 중일 때는 입력 무시
        lockOnSystem.ToggleLockOn();  // LockOnSystem의 ToggleLockOn 메서드 호출
    }

    // E 키 입력에 대한 타겟 전환 기능 추가
    void OnSwitchTarget()
    {
        if (isInteracting) return;  // 상호작용 중일 때는 입력 무시
        lockOnSystem.SwitchTarget();  // LockOnSystem의 SwitchTarget 메서드 호출
    }
}
