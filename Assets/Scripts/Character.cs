using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    [SerializeField] Transform characterBody;   // 캐릭터의 주요 신체
    [SerializeField] Transform followCam;       // 따라다니는 카메라
    Vector2 moveInput;                          // 이동 입력   
    Vector3 dodgeVec;                           // 회피 벡터
    Vector3 velocity;                           // 속도
    CharacterController characterController;    // 캐릭터 컨트롤러
    Animator animator;                          // 애니메이터 컴포넌트
    bool isRunning;                             // 달리는지 여부를 나타내는 플래그
    bool isDodging;                             // 회피 중인지 여부를 나타내는 플래그
    public bool isAttacking = false;            // 공격 중인지 여부를 나타내는 플래그
    float turnSmoothVelocity;
    public float playerSpeed = 5f;              // 캐릭터의 이동 속도
    public float sprintSpeed = 1.5f;            // 달리기 속도 배수
    public float smoothDampTime = 0.15f;        // 부드러운 회전을 위한 시간
    public float speedDampTime = 0.2f;
    float gravity = -9.8f;                      // 중력 힘        
    
    void Start()
    {
        // 필요한 컴포넌트 가져오기
        characterController = characterBody.GetComponent<CharacterController>();
        animator = characterBody.GetComponent<Animator>();
    }
    
    void Update()
    {
        
        Move();
        ApplyGravity();
        
    }

    // 캐릭터 이동 처리
    void Move()
    {
        // 공격 중이면 이동 금지
        if (IsAttacking() && !isDodging) return;

        float speed = isRunning ? sprintSpeed : 1f;
        animator.SetFloat("speed", moveInput.magnitude * speed, speedDampTime, Time.deltaTime);

        // 회피 중이면 회피 방향으로 캐릭터 이동
        if (isDodging)
        {
            speed = sprintSpeed;
            characterController.Move(dodgeVec * Time.deltaTime * playerSpeed * speed);
        } else
        {

            // 일반적인 이동 방향 계산 및 캐릭터 이동
            Vector3 moveDirection = CalculateMoveDirection();
            characterController.Move(moveDirection * Time.deltaTime * playerSpeed * speed);
            RotateCharacter(moveDirection); // 캐릭터 회전
        }
    }

     bool IsAttacking()
    {
        return (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") ||
                    animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") ||
                    animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3") || isAttacking);
    }

    // 이동 방향 계산
    Vector3 CalculateMoveDirection()
    {
        Vector3 lookForward = new Vector3(followCam.forward.x, 0f, followCam.forward.z).normalized;
        Vector3 lookRight = new Vector3(followCam.right.x, 0f, followCam.right.z).normalized;
        return lookForward * moveInput.y + lookRight * moveInput.x;
    }

    // 캐릭터 회전 처리
    void RotateCharacter(Vector3 moveDirection)
    {
        if (moveDirection != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            characterBody.rotation = Quaternion.Slerp(characterBody.rotation, newRotation, 0.15f);
        }
    }

    // 중력 적용
    void ApplyGravity()
    {
        if (!characterController.isGrounded) velocity.y += gravity * Time.deltaTime;
        else velocity.y = -0.5f;

        characterController.Move(velocity * Time.deltaTime);
    }

    // 이동 입력 핸들러
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // 달리기 입력 핸들러
    void OnSprint()
    {
        isRunning = !isRunning; // 달리기 토글
    }

    // 공격 입력 핸들러
    void OnAttack()
    {
        if (characterController.isGrounded && !isDodging)
        {
            isAttacking = true;            
            animator.SetTrigger("Attack");            
        }
    }

    // 회피 입력 핸들러
    void OnRoll()
    {
        if (moveInput.magnitude != 0 && !isDodging && characterController.isGrounded)
        {
            isDodging = true;
            dodgeVec = CalculateMoveDirection().normalized; // 이동 방향을 회피 방향으로 설정
            animator.SetTrigger("Dodge"); // 회피 애니메이션 재생
            characterController.center = new Vector3(0, 0.5f, 0); // 캐릭터 중심 변경
            characterController.height = 1f; // 캐릭터 높이 변경
            // 회피 시작 시 캐릭터를 회피 방향으로 바로 회전시킴
            characterBody.rotation = Quaternion.LookRotation(dodgeVec);
        }
    }
    
    void ResetAttack()=> isAttacking = false;   

    // 회피 애니메이션이 끝날 때 호출되는 이벤트
    void EndDodge()
    {
        // 회피 종료 시 캐릭터 중심과 높이를 초기화하고 회피 플래그를 비활성화함
        characterController.center = new Vector3(0, 0.88f, 0);
        characterController.height = 1.6f;
        isDodging = false;
        ResetAttack(); 
    }
}