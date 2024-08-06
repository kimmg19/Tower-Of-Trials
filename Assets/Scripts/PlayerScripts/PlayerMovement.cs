using System;
using UnityEngine;
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
    float smoothDampTime = 0.1f; // ȸ�� �ε巴�� �ϱ� ���� �ð�
    float speedDampTime = 0.2f; // �ӵ� ��ȭ �ε巴�� �ϱ� ���� �ð�

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
        // ���� ���̰� ȸ�� ���� �ƴ϶�� �̵����� ����
        if (animationEvent.IsAttacking() && !playerInputs.isDodging) return;

        // �̵� �ӵ��� �����ϰ� �ִϸ����Ϳ� �ӵ� �� ����
        float speed = playerInputs.isRunning ? playerStats.sprintSpeed : newSpeed;
        animator.SetFloat("speed", playerInputs.moveInput.magnitude * speed, speedDampTime, Time.deltaTime);

        if (playerInputs.isDodging)
        {
            // ȸ�� ���� �� �̵�
            speed = playerStats.sprintSpeed;
            characterController.Move(dodgeVec * Time.deltaTime * playerStats.playerSpeed * speed);
        } else
        {
            // �Ϲ����� �̵�
            Vector3 moveDirection = CalculateMoveDirection();
            characterController.Move(moveDirection * Time.deltaTime * playerStats.playerSpeed * speed);
            RotateCharacter(moveDirection);
        }
    }

    public Vector3 CalculateMoveDirection()
    {
        // ī�޶��� ���� �� ������ ������ �������� �̵� ���� ���
        Vector3 lookForward = new Vector3(followCam.forward.x, 0f, followCam.forward.z).normalized;
        Vector3 lookRight = new Vector3(followCam.right.x, 0f, followCam.right.z).normalized;
        return lookForward * playerInputs.moveInput.y + lookRight * playerInputs.moveInput.x;
    }

    void RotateCharacter(Vector3 moveDirection)
    {
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
        // ĳ���Ϳ� �߷� ����
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
