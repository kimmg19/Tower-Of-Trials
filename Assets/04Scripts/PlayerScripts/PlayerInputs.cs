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
    Shield shield;

    [SerializeField] GameObject buffParticlePrefab; // 버프 스킬 시 사용할 파티클 시스템 프리팹
    // 플레이어 상태 변수
    public Vector2 moveInput;
    // public bool isRunning = false;
    [HideInInspector] public bool isDodging;
    [HideInInspector] public bool isGPress;
    [HideInInspector] public bool isInteracting = false;
    [HideInInspector] public bool isBlocking = false;
    [HideInInspector] public bool isWalking = false;
    [HideInInspector] public bool isAttacking = false;
    public bool isSprinting = false;
    public bool isJumping = false;
    public bool isSkillAttacking = false;

    // 스태미나 및 쿨타임 변수
    [SerializeField] int blockInitialStamina = 3; // 방어 시작 시 처음 감소할 스태미나
    [SerializeField] int blockStamina = 2; // 방어 중 지속적으로 소모할 스태미나
    [SerializeField] float blockStaminaDecreaseInterval = 1f; // 방어 중 스태미나 감소 주기
    [SerializeField] float blockStaminaAcceleration = 0.8f; // 방어 지속 시 스태미나 감소 주기 가속
    [SerializeField] int sprintStamina = 1;
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
        // 코루틴 참조 변수
    private Coroutine blockStaminaCoroutine;
    Coroutine staminaCoroutine;

    void Start()
    {
         // 방패 오브젝트를 찾아서 Shield 컴포넌트 참조
        GameObject shieldObject = GameObject.FindGameObjectWithTag("Shield");
        if (shieldObject != null)
        {
            shield = shieldObject.GetComponent<Shield>(); // Shield
        }

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
        if (moveInput.magnitude == 0 && isSprinting)
        {
            StopSprinting();
        }
    }

    private void OnSprint(InputValue value)
    {
        // 방어 중에는 스프린트가 불가능하도록 처리
        if (isBlocking) return;

        isSprinting = value.isPressed;

        if (isSprinting && moveInput.magnitude != 0 && !isInteracting)
        {
            StartSprinting();
        }
        else
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

        isSprinting = false;
    }

    private IEnumerator StaminaCoroutine(int staminaUsage)
    {
        while (playerStats.currentStamina > 0 && isSprinting)
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
        if (isInteracting || animationEvent.IsAttacking()) return;

        animationEvent.OnFinishAttack();

        isBlocking = value.isPressed;
        animator.SetBool("Block", isBlocking);

        if (isBlocking)
        {
            if (playerStats.currentStamina >= blockInitialStamina)
            {
                playerStatus.UseStamina(blockInitialStamina);
                if (blockStaminaCoroutine != null)
                {
                    StopCoroutine(blockStaminaCoroutine);
                }
                blockStaminaCoroutine = StartCoroutine(BlockStaminaCoroutine());

                if (shield != null)
                {
                    shield.StartBlocking(); // 방어 시작
                    shield.ActivateParryWindow(); // 패링 윈도우 활성화
                }
            }
            else
            {
                StopBlocking();
            }

            StopSprinting();
        }
        else
        {
            StopBlocking();
        }
    }

    private IEnumerator BlockStaminaCoroutine()
    {
        // 방어 상태 유지 중 스태미나 감소
        float interval = blockStaminaDecreaseInterval;

        while (isBlocking && playerStats.currentStamina > 0)
        {
            yield return new WaitForSeconds(interval);
            playerStatus.UseStamina(blockStamina); // 지속적으로 스태미나 감소
            interval *= blockStaminaAcceleration; // 감소 주기 점점 빠르게
        }

        StopBlocking(); // 스태미나가 바닥나면 방어 중지
    }

    private void StopBlocking()
    {
        isBlocking = false;
        animator.SetBool("Block", false);

        if (blockStaminaCoroutine != null)
        {
            StopCoroutine(blockStaminaCoroutine);
            blockStaminaCoroutine = null;
        }

        playerMovement.SetBlockingMovement(false);

        if (shield != null)
        {
            shield.StopBlocking(); // 방어 종료
        }
    }

    void OnBuffSkill()
    {
        if (isInteracting) return;
        
        // 마나가 충분한지 확인
        if (playerStats.currentMp >= 30)
        {
            AudioManager.instance.Play("BuffSkill");
            // 애니메이션 트리거 설정
            animator.SetTrigger("BuffSkill");

            // 마나 소모
            playerStatus.UseMp(30);

            // 버프 스킬 실행
            StartCoroutine(ActivateBuffSkill());
        }
        else
        {
            Debug.Log("Not enough MP for Buff Skill.");
        }
    }

    private IEnumerator ActivateBuffSkill() // 버프 스킬 효과
    {
        // 버프 스킬이 시작되면서 상호작용 제한
        isInteracting = true;

        // 파티클 시스템 인스턴스화
        if (buffParticlePrefab != null)
        {
            GameObject particleSystem = Instantiate(buffParticlePrefab, transform.position, Quaternion.identity);
            particleSystem.transform.parent = this.transform; // 하이라키에 추가
            Destroy(particleSystem, 10f); // 10초 후 삭제 (버프 효과와 일치)
        }

        // 상호작용 제한 해제 (애니메이션 초기화 시점)
        yield return new WaitForSeconds(0.1f); // 잠깐의 대기 시간 후 상호작용 가능하게 설정
        isInteracting = false;

        // 공격력과 이동 속도 증가
        playerStats.IncreaseSwordDamage(10); // 예시로 공격력을 10 증가
        playerMovement.Buffspeed(); // 예시로 이동 속도를 증가시킵니다.

        // 일정 시간 후 효과를 원래대로 되돌림
        yield return new WaitForSeconds(10f); // 10초간 지속

        // 효과를 원래대로 되돌림
        playerStats.IncreaseSwordDamage(-10); // 공격력 감소
        playerMovement.Debuffspeed(); // 이동 속도 원래대로

        Debug.Log("Buff skill has ended.");
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
