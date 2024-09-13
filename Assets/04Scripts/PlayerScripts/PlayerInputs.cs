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
    public bool isJumping = false;
    public bool isSkillAttacking = false;

    // 스태미나 및 쿨타임 변수
    [SerializeField] int sprintStamina = 1;
    [SerializeField] int blockStamina = 1;
    [SerializeField] int jumpStamina = 5;
    [SerializeField] int rollStamina = 15;
    [SerializeField] int skillMp = 15;
    [SerializeField] float jumpCooldownDuration = 1.5f;
    [SerializeField] float rollCooldownDuration = 1f;
    [SerializeField] float skill01_CooldownDuration = 5f;    

    // UI 이미지 참조 (점프와 롤 쿨타임)
    [SerializeField] Image jumpCooldownImage;
    [SerializeField] Image rollCooldownImage;
    [SerializeField] Image skill01_CooldownImage;


    // 애니메이터 및 쿨타임 상태 변수
    Animator animator;
    bool isJumpCooldown = false;
    bool isRollCooldown = false;
    bool isSkill01Cooldown = false;

    // 스킬 파티클
    [SerializeField] ParticleSystem skillParticle_01;

    // 쿨타임 처리 코루틴 변수
    Coroutine staminaCoroutine;

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
        if (skill01_CooldownImage != null) skill01_CooldownImage.fillAmount = 0;

    }

    private void OnMove(InputValue value)
    {
        if (isInteracting) return;

        moveInput = value.Get<Vector2>();

        // 이동 입력이 0이면 스프린트 중지
        if (moveInput.magnitude == 0 && isRunning)
        {
            StopSprinting();
        }
    }

    private void OnSprint(InputValue value)
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
            staminaCoroutine = StartCoroutine(StaminaCoroutine(sprintStamina));
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

    private IEnumerator StaminaCoroutine(int staminaUsage)
    {
        while (playerStats.currentStamina > 0 && isRunning)
        {
            playerStatus.UseStamina(staminaUsage);
            yield return new WaitForSeconds(1.0f);
        }
        staminaCoroutine = null;
    }

    private void OnWalk(InputValue value)
    {
        if (isInteracting || moveInput.magnitude == 0) return;
        isWalking = value.isPressed;
    }

    private void OnAttack()
    {
        if (isInteracting || isBlocking || isSkillAttacking) return;

        if (playerMovement.characterController.isGrounded && !isDodging)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
        }
    }

    private void OnSkill01()
    {
        if (isInteracting || isBlocking || isDodging || isJumping
            || skillParticle_01.isPlaying || isAttacking|| isSkillAttacking|| isSkill01Cooldown) return;
        StartCoroutine(Skill01_CooldownCoroutine());
        playerStatus.UseMp(skillMp);
        animator.SetTrigger("Skill01");  }

    

    private void OnRoll()
    {
        if (isInteracting  || isDodging || isJumping || isRollCooldown || isSkillAttacking
            || moveInput.magnitude == 0|| playerStats.currentStamina < rollStamina) return;        

        playerMovement.Roll();
        playerStatus.UseStamina(rollStamina);

        StartCoroutine(RollCooldownCoroutine());
        isDodging = true;
    }

    private void OnInteraction()
    {
        StartCoroutine(InteractingCoroutine(() => isGPress = false));
        Debug.Log("G key pressed in PlayerInputs.");
    }

    private IEnumerator InteractingCoroutine(Action onComplete)
    {
        isGPress = true;
        yield return new WaitForSeconds(1f);
        onComplete?.Invoke();
    }

    private void OnPause()
    {
        if (isInteracting) return;
        inGameCanvas.GetComponent<InGameCanvas>().ClickPauseButton();
    }

    private void OnBlock(InputValue value)
    {
        if (isInteracting || animationEvent.IsAttacking() || isSkillAttacking) return;

        isBlocking = value.isPressed;

        if (isBlocking && playerStats.currentStamina > 0)
        {
            StartCoroutine(StaminaCoroutine(blockStamina));
        } else
        {
            StopCoroutine(StaminaCoroutine(blockStamina));
        }

        animator.SetBool("Block", isBlocking);
    }

    private void OnJump()
    {
        if (!isInteracting && playerMovement.characterController.isGrounded &&
            !isJumping && !animationEvent.IsAttacking() && !isDodging && !isJumpCooldown && !isSkillAttacking)
        {
            isJumping = true;
            if (playerStats.currentStamina < jumpStamina) return;            
            playerStatus.UseStamina(jumpStamina);
            StartCoroutine(JumpCooldownCoroutine());
            playerMovement.Jump();
        }
    }    

    private IEnumerator JumpCooldownCoroutine()
    {
        isJumpCooldown = true;
        yield return CooldownCoroutine(jumpCooldownDuration, jumpCooldownImage, () => isJumpCooldown = false);
    }

    private IEnumerator RollCooldownCoroutine()
    {
        isRollCooldown = true;
        yield return CooldownCoroutine(rollCooldownDuration, rollCooldownImage, () => isRollCooldown = false);
    }
    private IEnumerator Skill01_CooldownCoroutine()
    {
        isSkillAttacking = true;
        isSkill01Cooldown = true;
        yield return CooldownCoroutine(skill01_CooldownDuration,skill01_CooldownImage, () => isSkill01Cooldown = false);
    }

    private IEnumerator CooldownCoroutine(float duration, Image cooldownImage, Action onComplete)
    {
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

    private void OnLockOn()
    {
        if (isInteracting) return;
        lockOnSystem.ToggleLockOn();
    }

    private void OnSwitchTarget()
    {
        if (isInteracting) return;
        lockOnSystem.SwitchTarget();
    }
}
