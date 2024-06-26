using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform followCam;
    [HideInInspector] public Transform characterBody;
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Vector3 dodgeVec;
    Animator animator;
    AnimationEvent animationEvent;
    PlayerStats playerStats;
    PlayerStatus playerStatus;
    PlayerInputs playerInputs;
    Vector3 velocity;
    float turnSmoothVelocity;
    public float speed = 1.0f;
    float gravity = -9.8f;
    float smoothDampTime = 0.1f;
    float speedDampTime = 0.2f;

    void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        playerStats = GetComponent<PlayerStats>();
        playerInputs = GetComponent<PlayerInputs>();
        followCam = GameObject.Find("Main Camera").transform;
        characterBody = GameObject.Find("Player").transform;
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        animationEvent = GetComponent<AnimationEvent>();
    }

    void Update()
    {
        if (playerStatus.playerAlive)
        {
            Move(speed);
            ApplyGravity();
        }
    }

    public void Buffspeed()
    {
        speed += 0.5f;
        playerStats.sprintSpeed += 0.5f;
        Move(speed);
    }

    public void Debuffspeed()
    {
        speed -= 0.5f;
        playerStats.sprintSpeed -= 0.5f;
        Move(speed);
    }

    public void Move(float newSpeed)
    {
        if (animationEvent.IsAttacking() && !playerInputs.isDodging) return;

        float speed = playerInputs.isRunning ? playerStats.sprintSpeed : newSpeed;
        animator.SetFloat("speed", playerInputs.moveInput.magnitude * speed, speedDampTime, Time.deltaTime);

        if (playerInputs.isDodging)
        {
            speed = playerStats.sprintSpeed;
            characterController.Move(dodgeVec * Time.deltaTime * playerStats.playerSpeed * speed);
        } else
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

        if (!characterController.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        } else
        {
            velocity.y = -0.5f;
        }
        characterController.Move(velocity * Time.deltaTime);

    }
}
