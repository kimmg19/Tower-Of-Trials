using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerStats playerStats;
    PlayerInputs playerInputs;
    [SerializeField] public Transform characterBody;
    [SerializeField] Transform followCam;
    public CharacterController characterController;
    Animator animator;
    AnimationEvents animationEvents;
    float turnSmoothVelocity;
    Vector3 velocity;
    float speed = 1.0f;
    float gravity = -9.8f;
    float smoothDampTime = 0.1f;
    float speedDampTime = 0.2f;
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playerInputs = GetComponent<PlayerInputs>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        animationEvents = GetComponent<AnimationEvents>();
        // if (animationEvent == null || characterController == null || animator == null)
        // {
        //     Debug.LogError("Component not found!");
        // }
    }

    void Update()
    {
        if(playerStats.playerAlive){
        Move(speed); 
        ApplyGravity();
        }
    }

    public void Buffspeed()
    {
        speed += 0.5f;
        playerStats.sprintSpeed += 0.5f;
        Move(speed); // Move �޼��� ȣ�� �� ������ speed ���� ����
    }

    public void Debuffspeed()
    {
        speed -= 0.5f;
        playerStats.sprintSpeed -= 0.5f;
        Move(speed); // Move �޼��� ȣ�� �� ������ speed ���� ����
    }

    public void Move(float newSpeed)
    {
        if (animationEvents.IsAttacking() && !playerInputs.isDodging) return;

        float speed = playerInputs.isRunning ? playerStats.sprintSpeed : newSpeed; // ������ speed ���� ���
        animator.SetFloat("speed", playerInputs.moveInput.magnitude * speed, speedDampTime, Time.deltaTime);

        if (playerInputs.isDodging)
        {
            speed = playerStats.sprintSpeed;
            characterController.Move(playerInputs.dodgeVec * Time.deltaTime * playerStats.playerSpeed * speed);
        }
        else
        {
            Vector3 moveDirection = CalculateMoveDirection();
            characterController.Move(moveDirection * Time.deltaTime * playerStats.playerSpeed * speed);
            RotateCharacter(moveDirection);
        }
    }


    public Vector3 CalculateMoveDirection()
    {
        Vector3 lookForward = new Vector3(followCam.forward.x, 0f, followCam.forward.z).normalized;
        Vector3 lookRight = new Vector3(followCam.right.x, 0f, followCam.right.z).normalized;
        return lookForward * playerInputs.moveInput.y + lookRight * playerInputs.moveInput.x;
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

}

