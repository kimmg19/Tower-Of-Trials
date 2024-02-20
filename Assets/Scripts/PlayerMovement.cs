using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform characterBody; // 플레이어 캐릭터의 몸체(Transform)
    [SerializeField] Transform followCam; // 따라가는 카메라(Transform)
    Animator animator; // 애니메이터 컴포넌트
    public float playerSpeed = 5f; // 플레이어 이동 속도
    public float smoothDampTime = 0.15f; // 회전 시 부드러운 감속 시간
    public float gravity = -9.8f; // 중력 값
    public float attackMotionTime = 0.35f; // 공격 애니메이션 재생 시간
    public float shieldMotionTime = 0.35f; // 방패 애니메이션 재생 시간
    public float rollMotionTime = 0.5f; // 구르기 애니메이션 재생 시간
    public Vector2 moveInput { get; private set; } // 이동 입력 값
    Vector3 velocity; // 캐릭터 컨트롤러의 속도
    float turnSmoothVelocity; // 부드러운 회전을 위한 변수
    CharacterController characterController; // 캐릭터 컨트롤러 컴포넌트
    bool isAttacking = false; // 공격 중인지 여부
    bool isShielding = false; // 방패 사용 중인지 여부
    bool isRolling = false; // 구르기 중인지 여부

    void Start()
    {
        animator = characterBody.GetComponent<Animator>();
        characterController = characterBody.GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
        ApplyGravity();
    }

    void Move()
    {
        // 공격, 방패, 구르기 중이 아닐 때에만 이동 가능
        bool canMove = !isShielding && !isAttacking ;

        // 이동 중인지 확인하여 애니메이션 설정
        bool isMoving = canMove && moveInput.magnitude != 0;
        animator.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            // 이동 방향 설정
            Vector3 lookForward = new Vector3(followCam.forward.x, 0f, followCam.forward.z).normalized;
            Vector3 lookRight = new Vector3(followCam.right.x, 0f, followCam.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            // 이동 방향으로 회전 각도 계산
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            float currentAngle = Mathf.SmoothDampAngle(characterBody.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothDampTime);

            // 회전 적용
            characterBody.rotation = Quaternion.Euler(0f, currentAngle, 0f);

            if (canMove)
            {
                // 캐릭터 이동
                characterController.Move(moveDir * Time.deltaTime * playerSpeed);
            }
        }
    }

    void ApplyGravity()
    {
        // 캐릭터가 땅에 있지 않으면 중력 적용
        if (!characterController.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = -0.5f; // 중력 효과 적용
        }

        characterController.Move(velocity * Time.deltaTime);
    }

    // 이동 입력을 받는 콜백 함수
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // 공격 입력을 받는 콜백 함수
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && !isShielding && !isRolling)
        {
            // 공격 트리거 설정 및 공격 모션 재생
            isAttacking = true;
            animator.SetTrigger("onWeaponAttack");
            StartCoroutine(ResetAttackMotion());
        }
    }

    // 방패 입력을 받는 콜백 함수
    public void OnShield(InputAction.CallbackContext context)
    {
        if (context.performed && !isAttacking && !isRolling)
        {
            // 방패 트리거 설정 및 방패 모션 재생
            isShielding = true;
            animator.SetTrigger("onShield");
            StartCoroutine(ResetShieldMotion());
        }
        else if (context.canceled)
        {
            // 방패 입력이 취소되면 방패 사용 종료
            isShielding = false;
        }
    }

    // 구르기 입력을 받는 콜백 함수
    public void OnRoll(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // 구르기 트리거 설정 및 구르기 모션 재생
            isRolling = true;
            animator.SetTrigger("onRoll");
            StartCoroutine(ResetRollMotion());
        }
    }

    // 공격 모션 리셋을 위한 코루틴
    IEnumerator ResetAttackMotion()
    {
        yield return new WaitForSeconds(attackMotionTime);
        isAttacking = false;
    }

    // 방패 모션 리셋을 위한 코루틴
    IEnumerator ResetShieldMotion()
    {
        yield return new WaitForSeconds(shieldMotionTime);
        isShielding = false;
    }

    // 구르기 모션 리셋을 위한 코루틴
    IEnumerator ResetRollMotion()
    {
        yield return new WaitForSeconds(rollMotionTime);
        isRolling = false;
    }
}
