using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
//using UnityEngine.UIElements;

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
    [HideInInspector] public bool isDodging;
    [HideInInspector] public bool isGPress;
    [HideInInspector] public bool isInteracting = false;
    [HideInInspector] public bool canBlocking = false;
    [HideInInspector] public bool isWalking = false;
    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool isSkill_01_Unlocked = false;
    [HideInInspector] public bool isSkill_02_Unlocked = false;
    [HideInInspector] public string buffSkill = "BuffSkill";
    [HideInInspector] public string attackSkill = "AttackSkill";

    public bool isSprinting = false;
    public bool isJumping = false;
    public bool isSkillAttacking = false;

    // 스태미나 및 쿨타임 변수
    int blockInitialStamina = 3; // 방어 시작 시 처음 감소할 스태미나
    int blockStamina = 2; // 방어 중 지속적으로 소모할 스태미나
    float blockStaminaDecreaseInterval = 1f; // 방어 중 스태미나 감소 주기
     float blockStaminaAcceleration = 0.8f; // 방어 지속 시 스태미나 감소 주기 가속
    int sprintStamina = 1;
    int jumpStamina = 5;
    int rollStamina = 15;
    int attackSkillMp = 15;
    int buffSkillMp = 10;
    //쿨타임
    float jumpCooldownDuration = 1.5f;
    float rollCooldownDuration = 1f;
    float attackSkill_CooldownDuration = 5f;
    float buffSkill_CooldownDuration = 10f;
    //버픗 지속시간
    float buffSkillDuration = 10;

    // UI 이미지 참조 (점프와 롤 쿨타임이미지)
    [SerializeField] Image jumpCooldownImage;
    [SerializeField] Image rollCooldownImage;
    [SerializeField] Image attackSkill_CooldownImage;
    [SerializeField] GameObject attackSkill_UnlockImage;
    [SerializeField] Image buffSkill_CooldownImage;
    [SerializeField] GameObject buffSkill_UnlockImage;

    // 애니메이터 및 쿨타임 상태 변수
    Animator animator;
    bool isJumpCooldown = false;
    bool isRollCooldown = false;
    bool isAttackSkillCooldown = false;
    bool isBuffSkillCooldown = false;    
    [SerializeField] ParticleSystem attackSkillParticle;  
    private Coroutine blockStaminaCoroutine;
    Coroutine staminaCoroutine;

    private void Update()
    {
        if (CheckSkillUnlocked(attackSkill)==0)
        {
            attackSkill_UnlockImage.SetActive(true);
        } else
        {
            attackSkill_UnlockImage.SetActive(false);

        }
        if (CheckSkillUnlocked(buffSkill) == 0)
        {
            buffSkill_UnlockImage.SetActive(true);
        } else
        {
            buffSkill_UnlockImage.SetActive(false);

        }
    }
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
        if (attackSkill_CooldownImage != null) attackSkill_CooldownImage.fillAmount = 0;
        if (buffSkill_CooldownImage != null) buffSkill_CooldownImage.fillAmount = 0;

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
        if (canBlocking || playerStats.currentStamina < sprintStamina) return;

        isSprinting = value.isPressed;

        if (isSprinting && moveInput.magnitude != 0 && !isInteracting)
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
        isSprinting = false;
    }

    private IEnumerator StaminaCoroutine(int staminaUsage)
    {
        while (playerStats.currentStamina > sprintStamina && isSprinting)
        {
            playerStatus.UseStamina(staminaUsage);
            yield return new WaitForSeconds(1.0f);
        }
        StopSprinting();
        staminaCoroutine = null;
    }

    private void OnWalk(InputValue value)
    {
        if (isInteracting || moveInput.magnitude == 0) return;
        isWalking = value.isPressed;
    }    

    private void OnRoll()
    {
        if (isInteracting || isDodging || isJumping || isRollCooldown || isSkillAttacking
            || moveInput.magnitude == 0 || playerStats.currentStamina < rollStamina) return;

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

        canBlocking = value.isPressed;
        animator.SetBool("Block", canBlocking);

        if (canBlocking)
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
            } else
            {
                StopBlocking();
            }

            StopSprinting();
        } else
        {
            StopBlocking();
        }
    }

    private IEnumerator BlockStaminaCoroutine()
    {
        // 방어 상태 유지 중 스태미나 감소
        float interval = blockStaminaDecreaseInterval;

        while (canBlocking && playerStats.currentStamina > 0)
        {
            yield return new WaitForSeconds(interval);
            playerStatus.UseStamina(blockStamina); // 지속적으로 스태미나 감소
            interval *= blockStaminaAcceleration; // 감소 주기 점점 빠르게
        }

        StopBlocking(); // 스태미나가 바닥나면 방어 중지
    }

    private void StopBlocking()
    {
        canBlocking = false;
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
    private void OnAttack()
    {
        if (isInteracting || canBlocking || isSkillAttacking) return;

        if (playerMovement.characterController.isGrounded && !isDodging)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
        }
    }

    private void OnAttackSkill()
    {
        int skillUnlocked = CheckSkillUnlocked(attackSkill);
        print("공격 스킬 해제 여부: " + skillUnlocked);
        if (skillUnlocked == 1)
        {
            if (isInteracting || canBlocking || isDodging || isJumping
            || attackSkillParticle.isPlaying || isAttacking || isSkillAttacking || isAttackSkillCooldown) return;
            StartCoroutine(AttackSkill_CooldownCoroutine());
            if (playerStats.currentMp >= attackSkillMp)
            {
                playerStatus.UseMp(attackSkillMp);
                animator.SetTrigger("AttackSkill");
                StartCoroutine(ActiveAttackSkill());
            } else
            {
                Debug.Log("Not enough MP for Buff Skill.");
            }
        } else
        {
            print("공격 스킬 잠겨있음");
        }
    }
    private IEnumerator ActiveAttackSkill()
    {
        isInteracting = true;
        yield return new WaitForSeconds(1);
        isInteracting=false;
    }
    void OnBuffSkill()
    {
        int skillUnlocked = CheckSkillUnlocked(buffSkill);
        print("버프 스킬 해제 여부: " + skillUnlocked);
        if (skillUnlocked == 1)
        {
            if (isInteracting || isDodging || isJumping
            || attackSkillParticle.isPlaying || isAttacking || isSkillAttacking || isBuffSkillCooldown) return;
            StartCoroutine(BuffSkill_CooldownCoroutine());
            if (playerStats.currentMp >= buffSkillMp)
            {
                AudioManager.instance.Play("BuffSkill");
                animator.SetTrigger("BuffSkill");
                playerStatus.UseMp(buffSkillMp);
                StartCoroutine(ActivateBuffSkill());
            } else
            {
                Debug.Log("Not enough MP for Buff Skill.");
            }
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
            Destroy(particleSystem, 11.5f); // 10초 후 삭제 (버프 효과와 일치)
        }

        // 상호작용 제한 해제 (애니메이션 초기화 시점)
        yield return new WaitForSeconds(1.5f); // 잠깐의 대기 시간 후 상호작용 가능하게 설정
        isInteracting = false;

        // 공격력과 이동 속도 증가
        playerStats.IncreaseSwordDamage(10); // 예시로 공격력을 10 증가
        playerMovement.Buffspeed(); // 예시로 이동 속도를 증가시킵니다.

        // 일정 시간 후 효과를 원래대로 되돌림
        yield return new WaitForSeconds(buffSkillDuration); // 지속시간

        // 효과를 원래대로 되돌림
        playerStats.IncreaseSwordDamage(-10); // 공격력 감소
        playerMovement.Debuffspeed(); // 이동 속도 원래대로

        Debug.Log("Buff skill has ended.");
    }

    void OnJump()
    {
        if (!isInteracting && playerMovement.characterController.isGrounded &&
            !isJumping && !animationEvent.IsAttacking() && !isDodging && !isJumpCooldown && !isSkillAttacking)
        {
            if (playerStats.currentStamina < jumpStamina) return;
            isJumping = true;
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
    private IEnumerator AttackSkill_CooldownCoroutine()
    {
        isSkillAttacking = true;
        isAttackSkillCooldown = true;
        yield return CooldownCoroutine(attackSkill_CooldownDuration, attackSkill_CooldownImage, () => isAttackSkillCooldown = false);
    }
    private IEnumerator BuffSkill_CooldownCoroutine()
    {
        isBuffSkillCooldown = true;
        yield return CooldownCoroutine(buffSkill_CooldownDuration, buffSkill_CooldownImage, () => isBuffSkillCooldown = false);
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
        if (isInteracting || animationEvent.IsAttacking())
        {
            print("돌아가");
            return;
        }
        lockOnSystem.ToggleLockOn();
    }

    private void OnSwitchTarget()
    {
        if (isInteracting) return;
        lockOnSystem.SwitchTarget();
    }
    public int CheckSkillUnlocked(string skillName)
    {
        int isSkillUnlocked = PlayerPrefs.GetInt(skillName);
        return isSkillUnlocked;
    }
}
