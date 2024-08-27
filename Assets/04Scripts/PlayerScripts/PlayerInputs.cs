using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInputs : MonoBehaviour
{
    // 외부 컴포넌트 참조
    GameObject inGameCanvas;
    AnimationEvent animationEvent;
    LockOnSystem lockOnSystem;
    PlayerMovement playerMovement;
    PlayerStats playerStats;
    PlayerUI playerUI;
    PlayerStatus playerStatus;

    // 플레이어 상태 변수
    public Vector2 moveInput;
     public bool isRunning = false;
    [HideInInspector] public bool isDodging;
    [HideInInspector] public bool isGPress;
    [HideInInspector] public bool isInteracting = false;
    [HideInInspector] public bool isBlocking = false;
    [HideInInspector] public bool isWalking = false;
    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool isJumping = false;

    // 스태미나 및 쿨타임 변수
    [SerializeField] int strintStanima = 1;
    [SerializeField] int blockStanima = 1;
    [SerializeField] int jumpStamina = 5;
    [SerializeField] int rollStamina = 15;
    [SerializeField] float jumpCooldownDuration = 1.5f;
    [SerializeField] float rollCooldownDuration = 1f;

    // UI 이미지 참조 (점프와 롤 쿨타임)
    [SerializeField] Image jumpCooldownImage;
    [SerializeField] Image rollCooldownImage;

    // 애니메이터 및 쿨타임 상태 변수
    Animator animator;
    private bool isJumpCooldown = false;
    private bool isRollCooldown = false;

    void Start()
    {
        // 컴포넌트 초기화
        lockOnSystem = GetComponent<LockOnSystem>();
        playerStats = GetComponent<PlayerStats>();
        playerUI = GetComponent<PlayerUI>();
        playerMovement = GetComponent<PlayerMovement>();
        playerStatus = GetComponent<PlayerStatus>();
        inGameCanvas = GameObject.Find("InGameCanvas");
        animationEvent = GetComponent<AnimationEvent>();
        animator = GetComponent<Animator>();

        // 쿨타임 이미지 초기화
        if (jumpCooldownImage != null) jumpCooldownImage.fillAmount = 0;
        if (rollCooldownImage != null) rollCooldownImage.fillAmount = 0;
    }


    private Coroutine staminaCoroutine;

    void OnMove(InputValue value)
    {
        if (isInteracting) return;

        moveInput = value.Get<Vector2>();

        // 이동 입력이 0이 되면 스프린트 중지
        if (moveInput.magnitude == 0 && isRunning)
        {
            StopSprinting();
        }
    }

    IEnumerator StanimaCoroutine(int staminaUsage)
    {
        while (playerStats.currentStamina > 0 && isRunning)
        {
            playerStatus.UseStamina(staminaUsage);
            yield return new WaitForSeconds(1.0f);
        }

        staminaCoroutine = null;
    }

    void OnSprint(InputValue value)
    {
        isRunning = value.isPressed;

        if (isRunning && moveInput.magnitude != 0 && !isInteracting)
        {
            StartSprinting();
        } else
        {
            StopSprinting();
        }
    }
    private void StartSprinting()
    {
        if (staminaCoroutine == null)
        {
            staminaCoroutine = StartCoroutine(StanimaCoroutine(strintStanima));
        }
    }
    private void StopSprinting()
    {
        if (staminaCoroutine != null)
        {
            StopCoroutine(staminaCoroutine);
            staminaCoroutine = null;
        }

        isRunning = false;
    }
    void OnWalk(InputValue value)
    {
        if (isInteracting || moveInput.magnitude == 0) return;
        isWalking = value.isPressed;
    }

    void OnAttack()
    {
        if (isInteracting || isBlocking) return;

        if (playerMovement.characterController.isGrounded && !isDodging)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
        }
    }

    void OnRoll()
    {
        // 롤(구르기) 동작 처리 및 쿨타임 관리
        if (isInteracting || moveInput.magnitude == 0 || isDodging || isJumping || isRollCooldown) return;
        if (playerStats.currentStamina < rollStamina) return;

        playerMovement.Roll();
        playerStatus.UseStamina(rollStamina);

        StartCoroutine(RollCooldownCoroutine()); // 롤 쿨타임 시작
        isDodging = true;
    }

    void OnInteraction()
    {
        StartCoroutine(Interactting(() => isGPress = false));
        Debug.Log("G key pressed in PlayerInputs.");
    }

    IEnumerator Interactting(Action onComplete)
    {
        isGPress = true;
        yield return new WaitForSeconds(1f);
        onComplete?.Invoke();

    }

    void OnPause()
    {
        if (isInteracting) return;
        inGameCanvas.GetComponent<InGameCanvas>().ClickPauseButton();
    }

    void OnBlock(InputValue value)
    {
        // 방어 동작 처리 및 스태미나 관리
        if (isInteracting || animationEvent.IsAttacking()) return;
        animationEvent.OnFinishAttack();
        animationEvent.AtttackEffectOff();
        isBlocking = value.isPressed;

        if (isBlocking && playerStats.currentStamina > 0)
        {
            StartCoroutine(StanimaCoroutine(blockStanima));
        }
        else
        {
            StopCoroutine(StanimaCoroutine(blockStanima));
        }
        animator.SetBool("Block", isBlocking);
    }

    void OnParry()
    {
        // 패링 로직 (미구현 상태)
    }

    void OnJump()
    {
        // 점프 동작 처리 및 쿨타임 관리
        if (!isInteracting && playerMovement.characterController.isGrounded && !isJumping && !animationEvent.IsAttacking() && !isDodging && !isJumpCooldown)
        {
            if (playerStats.currentStamina < jumpStamina) return;

            StartCoroutine(JumpCoroutine()); // 점프 중 스태미나 관리
            StartCoroutine(JumpCooldownCoroutine()); // 점프 쿨타임 시작
            playerMovement.Jump();
        }
    }

    IEnumerator JumpCoroutine()
    {
        // 점프 상태 관리
        isJumping = true;
        playerStatus.UseStamina(jumpStamina);
        yield return new WaitForSeconds(0.2f); // 점프 중 스태미나 사용 대기 시간
        isJumping = false;
    }

    IEnumerator JumpCooldownCoroutine()
    {
        // 점프 쿨타임 관리
        isJumpCooldown = true;
        yield return CooldownCoroutine(jumpCooldownDuration, jumpCooldownImage, () => isJumpCooldown = false);
    }

    IEnumerator RollCooldownCoroutine()
    {
        // 구르기ㅣ 쿨타임 관리
        isRollCooldown = true;
        yield return CooldownCoroutine(rollCooldownDuration, rollCooldownImage, () => isRollCooldown = false);
    }

    IEnumerator CooldownCoroutine(float duration, Image cooldownImage, Action onComplete)
    {
        // 쿨타임 진행 및 이미지 업데이트
        float timer = 0f;
        cooldownImage.fillAmount = 1;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            cooldownImage.fillAmount = 1 - (timer / duration);
            yield return null;
        }

        cooldownImage.fillAmount = 0;
        onComplete?.Invoke();
    }

    void OnLockOn()
    {
        if (isInteracting) return;
        lockOnSystem.ToggleLockOn();
    }

    void OnSwitchTarget()
    {
        if (isInteracting) return;
        lockOnSystem.SwitchTarget();
    }
}
