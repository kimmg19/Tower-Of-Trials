using System;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    [SerializeField] Transform characterBody;
    [SerializeField] Transform followCam;
    Vector2 moveInput;
    Vector3 dodgeVec;
    Vector3 velocity;
    CharacterController characterController;
    Animator animator;
    bool isRunning;
    public bool isDodging;
    public bool isAttacking = false;
    public bool enableDamaging;
    float turnSmoothVelocity;
    public float playerSpeed = 5f;
    public float sprintSpeed = 1.5f;
    public float smoothDampTime = 0.15f;
    public float speedDampTime = 0.2f;
    float gravity = -9.8f;

    void Start()
    {
        characterController = characterBody.GetComponent<CharacterController>();
        animator = characterBody.GetComponent<Animator>();
    }

    void Update()
    {

        Move();
        ApplyGravity();

    }

    void Move()
    {
        if (IsAttacking() && !isDodging) return;

        float speed = isRunning ? sprintSpeed : 1f;
        animator.SetFloat("speed", moveInput.magnitude * speed, speedDampTime, Time.deltaTime);

        if (isDodging)
        {
            speed = sprintSpeed;
            characterController.Move(dodgeVec * Time.deltaTime * playerSpeed * speed);
        } else
        {

            Vector3 moveDirection = CalculateMoveDirection();
            characterController.Move(moveDirection * Time.deltaTime * playerSpeed * speed);
            RotateCharacter(moveDirection);
        }
    }

    public bool IsAttacking()
    {
        return (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") ||
                    animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") ||
                    animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3") || isAttacking);
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
            Quaternion newRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            characterBody.rotation = Quaternion.Slerp(characterBody.rotation, newRotation, 0.15f);
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
        print(moveInput);
    }

    void OnSprint()
    {
        isRunning = !isRunning;
    }

    void OnAttack()
    {
        if (characterController.isGrounded && !isDodging)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
        }
    }
    void DamageEnable()
    {
        enableDamaging = !enableDamaging;
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

    void OnFinishAttack() => isAttacking = false;

    void EndDodge()
    {
        characterController.center = new Vector3(0, 0.88f, 0);
        characterController.height = 1.6f;
        isDodging = false;
        OnFinishAttack();
    }
}