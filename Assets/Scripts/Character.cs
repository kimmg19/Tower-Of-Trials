using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    [SerializeField] Transform characterBody;   // 플레이어 캐릭터의 몸체
    [SerializeField] Transform followCam;       // 따라가는 카메라
    Vector2 moveInput;                          // 이동 입력   
    Vector3 velocity;
    CharacterController characterController;    // 캐릭터 컨트롤러
    Animator animator;                          // 애니메이터
    bool isRunning;                             // 달리는지 여부
    bool isDodging ;                            // 회피 중인지 여부
    public bool isAttacking = false;
    float turnSmoothVelocity;
    public float playerSpeed = 5f;              // 플레이어 이동 속도
    public float sprintSpeed = 1.5f;            // 플레이어 달리기 속도
    public float smoothDampTime = 0.15f;        // 회전 시 부드러운 감속 시간
    float gravity = -9.8f;                      // 중력        
    float timeSinceAttack = 0;
    int currentAttack = 0;                      //현재 attack

    void Start()
    {
        characterController = characterBody.GetComponent<CharacterController>();
        animator = characterBody.GetComponent<Animator>();
    }

    void Update()
    {
        timeSinceAttack += Time.deltaTime;
        Move();
        ApplyGravity();
    }

    void Move()
    {
        if (isAttacking && !isDodging) return;

        float speed = isRunning ? sprintSpeed : 1f;
        animator.SetFloat("speed", moveInput.magnitude * speed, 0.1f, Time.deltaTime);
        if (moveInput.magnitude != 0)
        {
            // 카메라 기준 이동 방향 설정
            Vector3 lookForward = new Vector3(followCam.forward.x, 0f, followCam.forward.z).normalized;
            Vector3 lookRight = new Vector3(followCam.right.x, 0f, followCam.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            // 캐릭터의 회전 각도 계산
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            float currentAngle = Mathf.SmoothDampAngle(characterBody.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothDampTime);
            characterBody.rotation = Quaternion.Euler(0f, currentAngle, 0f);

            // 이동 속도에 따라 캐릭터 이동
            characterController.Move(moveDir * Time.deltaTime * playerSpeed * speed);
        }
    }

    void ApplyGravity()
    {
        // 캐릭터가 땅에 있지 않으면 중력 적용
        if (!characterController.isGrounded) velocity.y += gravity * Time.deltaTime;
        else velocity.y = -0.5f;

        characterController.Move(velocity * Time.deltaTime);
    }

    // 이동 입력을 처리하는 함수
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // 달리기 입력을 처리하는 함수
    void OnSprint()
    {
        isRunning = !isRunning; // 달리기 여부 변경
    }

    void OnAttack()
    {
        //if (isAttacking) return;
        if (!isAttacking&&timeSinceAttack > 0.8f && characterController.isGrounded && !isDodging)
        {
            isAttacking = true;
            currentAttack++;
            if (currentAttack > 3) currentAttack = 1;
            if (timeSinceAttack > 1.0f) currentAttack = 1;
            animator.SetTrigger("Attack" + currentAttack);
            timeSinceAttack = 0;
        }
    }

    // 회피 입력을 처리하는 함수
    void OnRoll()
    {
        if (moveInput.magnitude != 0 && !isDodging)
        {
            isDodging = true;
            animator.SetTrigger("Dodge");
            characterController.center = new Vector3(0, 0.5f, 0);
            characterController.height = 1f;
        }
    }

    void ResetAttack()
    {
        isAttacking = false;
    }

    // 회피 애니메이션 종료 후 호출되는 이벤트 함수
    void EndDodge()
    {
        characterController.center = new Vector3(0, 0.88f, 0);
        characterController.height = 1.6f;
        isDodging = false;
        ResetAttack();
    }
}