using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    [SerializeField] Transform characterBody;   // �÷��̾� ĳ������ ��ü
    [SerializeField] Transform followCam;       // ���󰡴� ī�޶�
    Vector2 moveInput;                          // �̵� �Է�   
    Vector3 velocity;
    CharacterController characterController;    // ĳ���� ��Ʈ�ѷ�
    Animator animator;                          // �ִϸ�����
    bool isRunning;                             // �޸����� ����
    bool isDodging ;                            // ȸ�� ������ ����
    public bool isAttacking = false;
    float turnSmoothVelocity;
    public float playerSpeed = 5f;              // �÷��̾� �̵� �ӵ�
    public float sprintSpeed = 1.5f;            // �÷��̾� �޸��� �ӵ�
    public float smoothDampTime = 0.15f;        // ȸ�� �� �ε巯�� ���� �ð�
    float gravity = -9.8f;                      // �߷�        
    float timeSinceAttack = 0;
    int currentAttack = 0;                      //���� attack

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
            // ī�޶� ���� �̵� ���� ����
            Vector3 lookForward = new Vector3(followCam.forward.x, 0f, followCam.forward.z).normalized;
            Vector3 lookRight = new Vector3(followCam.right.x, 0f, followCam.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            // ĳ������ ȸ�� ���� ���
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            float currentAngle = Mathf.SmoothDampAngle(characterBody.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothDampTime);
            characterBody.rotation = Quaternion.Euler(0f, currentAngle, 0f);

            // �̵� �ӵ��� ���� ĳ���� �̵�
            characterController.Move(moveDir * Time.deltaTime * playerSpeed * speed);
        }
    }

    void ApplyGravity()
    {
        // ĳ���Ͱ� ���� ���� ������ �߷� ����
        if (!characterController.isGrounded) velocity.y += gravity * Time.deltaTime;
        else velocity.y = -0.5f;

        characterController.Move(velocity * Time.deltaTime);
    }

    // �̵� �Է��� ó���ϴ� �Լ�
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // �޸��� �Է��� ó���ϴ� �Լ�
    void OnSprint()
    {
        isRunning = !isRunning; // �޸��� ���� ����
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

    // ȸ�� �Է��� ó���ϴ� �Լ�
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

    // ȸ�� �ִϸ��̼� ���� �� ȣ��Ǵ� �̺�Ʈ �Լ�
    void EndDodge()
    {
        characterController.center = new Vector3(0, 0.88f, 0);
        characterController.height = 1.6f;
        isDodging = false;
        ResetAttack();
    }
}