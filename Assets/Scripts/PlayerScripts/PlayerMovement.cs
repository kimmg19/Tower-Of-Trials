using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    GameObject inGameCanvas;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Transform characterBody;
    [SerializeField] Transform followCam;
    public CharacterController characterController;
    Animator animator;
    AnimationEvent animationEvent;
    bool isRunning;
    public bool isDodging;
    float turnSmoothVelocity;
    Vector2 moveInput;
    Vector3 dodgeVec;
    Vector3 velocity;
    public bool isGPress;
    float speed = 1.0f;
    float gravity = -9.8f;
    [SerializeField] float smoothDampTime = 0.15f;
    [SerializeField] float speedDampTime = 0.2f;
    void Start()
    {
        inGameCanvas = GameObject.Find("InGameCanvas");
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        animationEvent = GetComponent<AnimationEvent>();
        if (animationEvent == null || characterController == null || animator == null)
        {
            Debug.LogError("Component not found!");
        }
    }

    void Update()
    {
        Move(speed); // 수정된 속도 값을 전달하여 Move 호출
        ApplyGravity();
    }

    public void Buffspeed()
    {
        speed += 0.5f;
        playerStats.sprintSpeed += 0.5f;
        Move(speed); // Move 메서드 호출 시 수정된 speed 값을 전달
    }

    public void Debuffspeed()
    {
        speed -= 0.5f;
        playerStats.sprintSpeed -= 0.5f;
        Move(speed); // Move 메서드 호출 시 수정된 speed 값을 전달
    }

    public void Move(float newSpeed)
    {
        if (animationEvent.IsAttacking() && !isDodging) return;

        float speed = isRunning ? playerStats.sprintSpeed : newSpeed; // 수정된 speed 값을 사용
        animator.SetFloat("speed", moveInput.magnitude * speed, speedDampTime, Time.deltaTime);

        if (isDodging)
        {
            speed = playerStats.sprintSpeed;
            characterController.Move(dodgeVec * Time.deltaTime * playerStats.playerSpeed * speed);
        }
        else
        {
            Vector3 moveDirection = CalculateMoveDirection();
            characterController.Move(moveDirection * Time.deltaTime * playerStats.playerSpeed * speed);
            RotateCharacter(moveDirection);
        }
    }


    Vector3 CalculateMoveDirection()
    {
        Vector3 lookForward = new Vector3(followCam.forward.x, 0f, followCam.forward.z).normalized;
        Vector3 lookRight = new Vector3(followCam.right.x, 0f, followCam.right.z).normalized;
        return lookForward * moveInput.y + lookRight * moveInput.x;
    }

    void RotateCharacter(Vector3 moveDirection)
    {
        if (moveDirection != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float currentAngle = Mathf.SmoothDampAngle(characterBody.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothDampTime);
            characterBody.rotation = Quaternion.Euler(0f, currentAngle, 0f);
        }
    }

    void ApplyGravity()
    {
        if (!characterController.isGrounded) velocity.y += gravity * Time.deltaTime;
        else velocity.y = -0.5f;

        characterController.Move(velocity * Time.deltaTime);
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnSprint()
    {
        isRunning = !isRunning;
    }

    void OnAttack()
    {
        if (characterController.isGrounded && !isDodging)
        {
            animationEvent.isAttacking = true;
            animator.SetTrigger("Attack");
        }
    }
    void OnRoll()
    {
        if (moveInput.magnitude != 0 && !isDodging && characterController.isGrounded)
        {
            isDodging = true;
            AudioManager.instance.Play("PlayerRoll");
            dodgeVec = CalculateMoveDirection().normalized;
            animator.SetTrigger("Dodge");
            characterController.center = new Vector3(0, 0.5f, 0);
            characterController.height = 1f;
            characterBody.rotation = Quaternion.LookRotation(dodgeVec);
        }
    }
    void OnInteraction()
    {
        
        isGPress = true;

    }
    void OnPause()
    {
        inGameCanvas.GetComponent<InGameCanvas>().ClickPuaseButton();
    }
}

