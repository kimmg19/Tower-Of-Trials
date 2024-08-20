using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform followCam; // ī�޶��� Transform�� ����
    [HideInInspector] public Transform characterBody; // ĳ������ Transform�� ����
    [HideInInspector] public CharacterController characterController; // ĳ���� ��Ʈ�ѷ��� ����
    [HideInInspector] public Vector3 dodgeVec; // ȸ�� ���� ����
    Animator animator; // �ִϸ����� ������Ʈ
    AnimationEvent animationEvent; // �ִϸ��̼� �̺�Ʈ ������Ʈ
    PlayerStats playerStats; // �÷��̾��� ��� ������
    PlayerStatus playerStatus; // �÷��̾��� ���� ������
    PlayerInputs playerInputs; // �÷��̾��� �Է� ������
    Vector3 velocity; // �ӵ� ����
    float turnSmoothVelocity; // ȸ�� �ε巴�� �ϱ� ���� ����
    public float speed = 1.0f; // �⺻ �ӵ�
    float gravity = -9.8f; // �߷� ��
    float jumpHeight = 2f; // ���� ����
    float smoothDampTime = 0.1f; // ȸ�� �ε巴�� �ϱ� ���� �ð�
    float speedDampTime = 0.1f; // �ӵ� ��ȭ �ε巴�� �ϱ� ���� �ð�
    LockOnSystem lockOnSystem;

    void Start()
    {
        // �ʿ��� ������Ʈ�� ������Ʈ���� �ʱ�ȭ
        playerStatus = GetComponent<PlayerStatus>();
        playerStats = GetComponent<PlayerStats>();
        playerInputs = GetComponent<PlayerInputs>();
        followCam = GameObject.Find("Main Camera").transform; // ���� ī�޶� ã��
        characterBody = GameObject.Find("Player").transform; // �÷��̾� ������Ʈ ã��
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        animationEvent = GetComponent<AnimationEvent>();
        lockOnSystem = GetComponent<LockOnSystem>();
    }

    void Update()
    {

        // �÷��̾ ����ְ� ��ȣ�ۿ� ���� �ƴ� �� �̵� �� �߷� ����
        if (playerStatus.playerAlive && !playerInputs.isInteracting)
        {
            Move(speed);
            ApplyGravity();
        }
    }

    public void Buffspeed()
    {
        // �ӵ� ���� �� �̵� ������Ʈ
        speed += 0.5f;
        playerStats.sprintSpeed += 0.5f;
        Move(speed);
    }

    public void Debuffspeed()
    {
        // �ӵ� ���� �� �̵� ������Ʈ
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

        float speed = playerInputs.isRunning ? playerStats.sprintSpeed : newSpeed;
        Vector3 moveDirection = CalculateMoveDirection();

        if (!lockOnSystem.isLockOn)
        {
            animator.SetFloat("speed", playerInputs.moveInput.magnitude * speed, speedDampTime, Time.deltaTime);
        } else if (lockOnSystem.isLockOn)
        {
            // ĳ������ ���� ���⿡���� �Է� ������ ���
            Vector3 localMove = characterBody.InverseTransformDirection(moveDirection);

            // �ִϸ����� �Ķ���Ϳ� ���� ��ǥ�� �������� ���� ����
            animator.SetFloat("Horizontal", localMove.x * speed, speedDampTime, Time.deltaTime);
            animator.SetFloat("Vertical", localMove.z * speed, speedDampTime, Time.deltaTime);
        }

        if (playerInputs.isDodging)
        {
            MoveCharacter(dodgeVec, playerStats.sprintSpeed);
        } else
        {
            MoveCharacter(moveDirection, speed);
            if (!lockOnSystem.isLockOn)
            {
                RotateCharacter(moveDirection);
            }
        }
    }

    // ĳ���� �̵� �Լ�
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
        // �̵� ���⿡ ���� ĳ���� ȸ��
        if (moveDirection != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float currentAngle = Mathf.SmoothDampAngle(characterBody.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothDampTime);
            characterBody.rotation = Quaternion.Euler(0f, currentAngle, 0f);
        }
    }

    void ApplyGravity()
    {
        // ĳ���Ͱ� ���鿡 ��� �ִ��� Ȯ��
        if (!characterController.isGrounded)
        {
            // ���߿� ���� ���� �߷� ����
            velocity.y += gravity * Time.deltaTime * 2;
        } else if (velocity.y < 0)
        {
            // ���鿡 ���� ���� ���� �ӵ��� �ּҰ����� ����
            velocity.y = -0.5f;
        }

        // ���� �ӵ��� ĳ���� �̵�
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
            playerStatus.UseStamina(15);  // ���¹̳� ����
            AudioManager.instance.Play("PlayerRoll");
            dodgeVec = CalculateMoveDirection().normalized;
            animator.SetTrigger("Dodge");
            characterController.center = new Vector3(0, 0.5f, 0);
            characterController.height = 1f;
            characterBody.rotation = Quaternion.LookRotation(dodgeVec);
        }
    }
}
