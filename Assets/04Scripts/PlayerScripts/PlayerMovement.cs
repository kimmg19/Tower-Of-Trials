using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform followCam; // 카메라의 Transform을 설정
    [HideInInspector] public Transform characterBody; // 캐릭터의 Transform을 설정
    [HideInInspector] public CharacterController characterController; // 캐릭터 컨트롤러를 설정
    [HideInInspector] public Vector3 dodgeVec; // 회피 방향 벡터
    Animator animator; // 애니메이터 컴포넌트
    AnimationEvent animationEvent; // 애니메이션 이벤트 컴포넌트
    PlayerStats playerStats; // 플레이어의 통계 데이터
    PlayerStatus playerStatus; // 플레이어의 상태 데이터
    PlayerInputs playerInputs; // 플레이어의 입력 데이터
    Vector3 velocity; // 속도 벡터
    float turnSmoothVelocity; // 회전 부드럽게 하기 위한 변수
    public float speed = 1.0f; // 기본 속도
    float gravity = -9.8f; // 중력 값
    float jumpHeight = 2f; // 점프 높이
    float smoothDampTime = 0.1f; // 회전 부드럽게 하기 위한 시간
    float speedDampTime = 0.1f; // 속도 변화 부드럽게 하기 위한 시간
    LockOnSystem lockOnSystem;

    void Start()
    {
        // 필요한 컴포넌트와 오브젝트들을 초기화
        playerStatus = GetComponent<PlayerStatus>();
        playerStats = GetComponent<PlayerStats>();
        playerInputs = GetComponent<PlayerInputs>();
        followCam = GameObject.Find("Main Camera").transform; // 메인 카메라 찾기
        characterBody = GameObject.Find("Player").transform; // 플레이어 오브젝트 찾기
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        animationEvent = GetComponent<AnimationEvent>();
        lockOnSystem = GetComponent<LockOnSystem>();
    }

    void Update()
    {

        // 플레이어가 살아있고 상호작용 중이 아닐 때 이동 및 중력 적용
        if (playerStatus.playerAlive && !playerInputs.isInteracting)
        {
            Move(speed);
            ApplyGravity();
        }
    }

    public void Buffspeed()
    {
        // 속도 증가 및 이동 업데이트
        speed += 0.5f;
        playerStats.sprintSpeed += 0.5f;
        Move(speed);
    }

    public void Debuffspeed()
    {
        // 속도 감소 및 이동 업데이트
        speed -= 0.5f;
        playerStats.sprintSpeed -= 0.5f;
        Move(speed);
    }

    public void Move(float newSpeed)
    {
        if (animationEvent.IsAttacking() && !playerInputs.isDodging) return;

        if (playerInputs.isWalking)
        {
            newSpeed = playerStats.walkSpeed;
        }

        float speed = (playerInputs.isRunning) ? playerStats.sprintSpeed : newSpeed;
        Vector3 moveDirection = CalculateMoveDirection();

        if (!lockOnSystem.isLockOn)
        {
            animator.SetFloat("speed", playerInputs.moveInput.magnitude * speed, speedDampTime, Time.deltaTime);
        }
        else if (lockOnSystem.isLockOn)
        {
            // 캐릭터의 로컬 방향에서의 입력 방향을 계산
            Vector3 localMove = characterBody.InverseTransformDirection(moveDirection);

            // 애니메이터 파라미터에 로컬 좌표계 기준으로 값을 전달
            animator.SetFloat("Horizontal", localMove.x * speed, speedDampTime, Time.deltaTime);
            animator.SetFloat("Vertical", localMove.z * speed, speedDampTime, Time.deltaTime);
        }

        if (playerInputs.isDodging)
        {
            MoveCharacter(dodgeVec, playerStats.sprintSpeed);
        }
        else
        {
            MoveCharacter(moveDirection, speed);
            if (!lockOnSystem.isLockOn)
            {
                RotateCharacter(moveDirection);
            }
        }
    }

    // 캐릭터 이동 함수
    void MoveCharacter(Vector3 direction, float speed)
    {
        characterController.Move(direction * Time.deltaTime * playerStats.playerSpeed * speed);
    }

    public Vector3 CalculateMoveDirection()
    {
        Vector3 lookForwardY = new Vector3(followCam.forward.x, 0f, followCam.forward.z).normalized;
        Vector3 lookForwardX = new Vector3(followCam.right.x, 0f, followCam.right.z).normalized;
        return lookForwardY * playerInputs.moveInput.y + lookForwardX * playerInputs.moveInput.x;
    }

    void RotateCharacter(Vector3 moveDirection)
    {
        if (animator.runtimeAnimatorController == lockOnSystem.playerAnimator_LockOn) return;
        // 이동 방향에 따라 캐릭터 회전
        if (moveDirection != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float currentAngle = Mathf.SmoothDampAngle(characterBody.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothDampTime);
            characterBody.rotation = Quaternion.Euler(0f, currentAngle, 0f);
        }
    }

    void ApplyGravity()
    {
        // 캐릭터가 지면에 닿아 있는지 확인
        if (!characterController.isGrounded)
        {
            // 공중에 있을 때만 중력 적용
            velocity.y += gravity * Time.deltaTime * 2;
        }
        else if (velocity.y < 0)
        {
            // 지면에 있을 때는 수직 속도를 최소값으로 고정
            velocity.y = -0.5f;
        }

        // 계산된 속도로 캐릭터 이동
        characterController.Move(velocity * Time.deltaTime);
    }

    public void Jump()
    {

        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        animator.SetTrigger("Jump");

    }
    public void Roll()
    {
        if (characterController.isGrounded)
        {
            animationEvent.OnFinishAttack();
            animationEvent.AtttackEffectOff();
            AudioManager.instance.Play("PlayerRoll");
            dodgeVec = CalculateMoveDirection().normalized;
            animator.SetTrigger("Dodge");
            characterController.center = new Vector3(0, 0.5f, 0);
            characterController.height = 1f;
            characterBody.rotation = Quaternion.LookRotation(dodgeVec);
        }
    }
}
