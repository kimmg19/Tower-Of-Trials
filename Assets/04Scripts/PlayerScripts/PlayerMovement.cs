using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform followCam; // 카메라 Transform 참조
    [HideInInspector] public Transform characterBody; // 캐릭터의 Transform 참조
    [HideInInspector] public Vector3 dodgeVec; // 회피 방향 벡터
    [SerializeField] public CharacterController characterController;
    Animator animator; // 애니메이터 참조
    AnimationEvent animationEvent; // 애니메이션 이벤트 참조
    PlayerStats playerStats; // 플레이어 스탯 관리
    PlayerStatus playerStatus; // 플레이어 상태 관리
    PlayerInputs playerInputs; // 플레이어 입력 관리
    Vector3 velocity; // 이동 속도 벡터
    float turnSmoothVelocity; // 회전 부드럽게 전환할 때 필요한 변수
    public float speed = 1.0f; // 기본 이동 속도
    float gravity = -9.8f; // 중력 값
    float jumpHeight = 2f; // 점프 높이
    float smoothDampTime = 0.1f; // 회전 부드럽게 전환할 때 필요한 시간
    float speedDampTime = 0.1f; // 속도 변화 부드럽게 전환할 때 필요한 시간
    LockOnSystem lockOnSystem;

    // 방어 중 이동 속도를 줄이기 위한 변수
    private float blockingSpeedMultiplier = 0.5f; // 방어 시 이동 속도 감소 비율
    private bool isBlocking = false; // 현재 방어 상태인지 여부

    void Start()
    {
        // 필요한 컴포넌트와 스크립트들을 초기화
        playerStatus = GetComponent<PlayerStatus>();
        playerStats = GetComponent<PlayerStats>();
        playerInputs = GetComponent<PlayerInputs>();
        followCam = GameObject.Find("Main Camera").transform; // 메인 카메라 찾기
        characterBody = GameObject.Find("Player").transform; // 플레이어 트랜스폼 찾기
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        animationEvent = GetComponent<AnimationEvent>();
        lockOnSystem = GetComponent<LockOnSystem>();
    }
    
    void Update()
    {
        // 플레이어가 살아있고 상호작용 중이 아닌 경우 이동과 중력 적용
        if (playerStatus.playerAlive && !playerInputs.isInteracting)
        {
            Move(speed);
            ApplyGravity();
            Debug.Log(velocity.y);
        }
    }

    public void Buffspeed()
    {
        // 이동 속도 증가 및 이동 갱신
        speed += 0.5f;
        playerStats.sprintSpeed += 0.5f;
        Move(speed);
    }

    public void Debuffspeed()
    {
        // 이동 속도 감소 및 이동 갱신
        speed -= 0.5f;
        playerStats.sprintSpeed -= 0.5f;
        Move(speed);
    }

    public void Move(float newSpeed)
    {
        if ((animationEvent.IsAttacking() && !playerInputs.isDodging)|| playerInputs.isSkillAttacking) return;

        // 방어 중인 경우 이동 속도 감소
        if (isBlocking)
        {
            newSpeed *= blockingSpeedMultiplier;
            playerInputs.isSprinting = false; // 방어 중에는 달리기 불가능
        }

        if (playerInputs.isWalking)
        {
            newSpeed = playerStats.walkSpeed;
        }

        float speed = (playerInputs.isSprinting && !isBlocking) ? playerStats.sprintSpeed : newSpeed; // 방어 중이면 스프린트 불가
        Vector3 moveDirection = CalculateMoveDirection();

        if (!lockOnSystem.isLockOn)
        {
            animator.SetFloat("speed", playerInputs.moveInput.magnitude * speed, speedDampTime, Time.deltaTime);
        }
        else if (lockOnSystem.isLockOn)
        {
            // 캐릭터의 로컬 방향에서 입력 벡터 계산
            Vector3 localMove = characterBody.InverseTransformDirection(moveDirection);

            // 애니메이터 파라미터에 수평 및 수직 입력 반영
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

    public void ApplyGravity()
    {
        bool isGrounded = CheckGrounded();

        if (isGrounded)
        {
            // 지면에 있을 때 중력 초기화
            if (velocity.y < 0)
            {
                velocity.y = -2.0f;
            }
            Debug.Log("지면에 있음");
        }
        else
        {
            // 공중에 있을 때 중력 적용
            velocity.y += gravity * Time.deltaTime * 2;
            Debug.Log("지면에 있지 않음");
        }

        // 속도에 따라 캐릭터 이동
        characterController.Move(velocity * Time.deltaTime);
    }

    private bool CheckGrounded()
    {
        // 기본 Raycast (정확한 아래 방향)
        if (characterController.isGrounded) return true;

        // 여러 방향에서 Raycast
        Vector3 origin = transform.position;
        float checkDistance = 0.1f;

        // 정중앙 아래 방향
        if (Physics.Raycast(origin, Vector3.down, checkDistance)) return true;

        // 약간 기울어진 방향으로 Raycast
        Vector3[] directions = {
        new Vector3(-0.1f, -1f, 0), // 왼쪽
        new Vector3(0.1f, -1f, 0),  // 오른쪽
        new Vector3(-0.1f, -1f, -0.1f), // 왼쪽 뒤쪽
        new Vector3(0.1f, -1f, -0.1f),  // 오른쪽 뒤쪽
        new Vector3(-0.1f, -1f, 0.1f), // 왼쪽 앞쪽
        new Vector3(0.1f, -1f, 0.1f)   // 오른쪽 앞쪽
    };

        foreach (var direction in directions)
        {
            if (Physics.Raycast(origin + direction, Vector3.down, checkDistance))
            {
                return true;
            }
        }

        return false;
    }

    /*
    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime * 2;
        // 속도에 따라 캐릭터 이동
        characterController.Move(velocity * Time.deltaTime);
    }
    */
    //velocity.y가 중첩되지않도록 초기화 / 중첩되면 빠르게 떨어짐. Jump하면 초기화하기때문에 괜찮았던 것
    public void VelocityNormalize()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    public void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        AudioManager.instance.Play("PlayerJump");
        animator.SetTrigger("Jump");
        //Debug.Log(gravity);
    }

    public void Roll()
    {
        if (characterController.isGrounded)
        {
            animationEvent.OnFinishAttack();
            AudioManager.instance.Play("PlayerRoll");
            dodgeVec = CalculateMoveDirection().normalized;
            animator.SetTrigger("Dodge");
            characterController.center = new Vector3(0, 0.5f, 0);
            characterController.height = 1f;
            characterBody.rotation = Quaternion.LookRotation(dodgeVec);
        }
    }

    // 방어 중 이동 속도 조절 메서드
    public void SetBlockingMovement(bool isBlocking)
    {
        this.isBlocking = isBlocking;
        if (isBlocking)
        {
            // 방어 중일 때 스태미너 소모 방지
            playerInputs.isSprinting = false; // 스프린트 비활성화
        }
    }
}
