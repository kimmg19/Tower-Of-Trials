using System;
using UnityEngine;
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
    float smoothDampTime = 0.1f; // 회전 부드럽게 하기 위한 시간
    float speedDampTime = 0.2f; // 속도 변화 부드럽게 하기 위한 시간

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
        // 공격 중이고 회피 중이 아니라면 이동하지 않음
        if (animationEvent.IsAttacking() && !playerInputs.isDodging) return;

        // 이동 속도를 설정하고 애니메이터에 속도 값 전달
        float speed = playerInputs.isRunning ? playerStats.sprintSpeed : newSpeed;
        animator.SetFloat("speed", playerInputs.moveInput.magnitude * speed, speedDampTime, Time.deltaTime);

        if (playerInputs.isDodging)
        {
            // 회피 중일 때 이동
            speed = playerStats.sprintSpeed;
            characterController.Move(dodgeVec * Time.deltaTime * playerStats.playerSpeed * speed);
        } else
        {
            // 일반적인 이동
            Vector3 moveDirection = CalculateMoveDirection();
            characterController.Move(moveDirection * Time.deltaTime * playerStats.playerSpeed * speed);
            RotateCharacter(moveDirection);
        }
    }

    public Vector3 CalculateMoveDirection()
    {
        // 카메라의 전방 및 오른쪽 방향을 기준으로 이동 방향 계산
        Vector3 lookForward = new Vector3(followCam.forward.x, 0f, followCam.forward.z).normalized;
        Vector3 lookRight = new Vector3(followCam.right.x, 0f, followCam.right.z).normalized;
        return lookForward * playerInputs.moveInput.y + lookRight * playerInputs.moveInput.x;
    }

    void RotateCharacter(Vector3 moveDirection)
    {
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
        // 캐릭터에 중력 적용
        if (!characterController.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime * 2;
        } else
        {
            velocity.y = -0.5f;
        }
        characterController.Move(velocity * Time.deltaTime);
    }
}
