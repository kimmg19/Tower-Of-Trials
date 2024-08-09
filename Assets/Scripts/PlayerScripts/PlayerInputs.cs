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
    PlayerStatus playerStatus;
    LockOnSystem lockOnSystem;  // LockOnSystem 스크립트 참조 추가
    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public bool isRunning = false;
    [HideInInspector] public bool isDodging;
    [HideInInspector] public bool isGPress;
    [HideInInspector] public bool isInteracting = false;
    Animator animator;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playerUI = GetComponent<PlayerUI>();
        playerMovement = GetComponent<PlayerMovement>();
        playerStatus = GetComponent<PlayerStatus>();
        inGameCanvas = GameObject.Find("InGameCanvas");
        animationEvent = GetComponent<AnimationEvent>();
        animator = GetComponent<Animator>();
        lockOnSystem = GetComponent<LockOnSystem>();  // LockOnSystem 스크립트 초기화
    }

    void OnMove(InputValue value)
    {
        if (isInteracting) return;  // 상호작용 중일 때는 입력 무시
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

    void OnSprint()
    {
        if (isInteracting) return;  // 상호작용 중일 때는 입력 무시
        if (!isRunning && playerStats.currentStamina > 0)
        {
            isRunning = true;
            StartCoroutine(SprintCoroutine());
        } 
        else
        {
            isRunning = false;
            StopCoroutine(SprintCoroutine());
        }
    }

    void OnAttack()
    {
        if (isInteracting) return;  // 상호작용 중일 때는 입력 무시
        if (playerMovement.characterController.isGrounded && !isDodging)
        {
            animationEvent.isAttacking = true;
            animator.SetTrigger("Attack");
        }
    }

    void OnRoll()
    {
        if (isInteracting) return;  // 상호작용 중일 때는 입력 무시
        if (moveInput.magnitude != 0 && !isDodging && playerMovement.characterController.isGrounded)
        {
            if (playerStats.currentStamina >= 15) // 스태미나가 충분한지 확인
            {
                animationEvent.OnFinishAttack();
                animationEvent.AtttackEffectOff();
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

    // Q 키 입력에 대한 LockOn 기능 호출
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
