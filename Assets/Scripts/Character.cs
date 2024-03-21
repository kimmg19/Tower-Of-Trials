using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    [SerializeField] Transform characterBody;   // ĳ������ �ֿ� ��ü
    [SerializeField] Transform followCam;       // ����ٴϴ� ī�޶�
    Vector2 moveInput;                          // �̵� �Է�   
    Vector3 dodgeVec;                           // ȸ�� ����
    Vector3 velocity;                           // �ӵ�
    CharacterController characterController;    // ĳ���� ��Ʈ�ѷ�
    Animator animator;                          // �ִϸ����� ������Ʈ
    bool isRunning;                             // �޸����� ���θ� ��Ÿ���� �÷���
    bool isDodging;                             // ȸ�� ������ ���θ� ��Ÿ���� �÷���
    public bool isAttacking = false;            // ���� ������ ���θ� ��Ÿ���� �÷���
    float turnSmoothVelocity;
    public float playerSpeed = 5f;              // ĳ������ �̵� �ӵ�
    public float sprintSpeed = 1.5f;            // �޸��� �ӵ� ���
    public float smoothDampTime = 0.15f;        // �ε巯�� ȸ���� ���� �ð�
    public float speedDampTime = 0.2f;
    float gravity = -9.8f;                      // �߷� ��        
    
    void Start()
    {
        // �ʿ��� ������Ʈ ��������
        characterController = characterBody.GetComponent<CharacterController>();
        animator = characterBody.GetComponent<Animator>();
    }
    
    void Update()
    {
        
        Move();
        ApplyGravity();
        
    }

    // ĳ���� �̵� ó��
    void Move()
    {
        // ���� ���̸� �̵� ����
        if (IsAttacking() && !isDodging) return;

        float speed = isRunning ? sprintSpeed : 1f;
        animator.SetFloat("speed", moveInput.magnitude * speed, speedDampTime, Time.deltaTime);

        // ȸ�� ���̸� ȸ�� �������� ĳ���� �̵�
        if (isDodging)
        {
            speed = sprintSpeed;
            characterController.Move(dodgeVec * Time.deltaTime * playerSpeed * speed);
        } else
        {

            // �Ϲ����� �̵� ���� ��� �� ĳ���� �̵�
            Vector3 moveDirection = CalculateMoveDirection();
            characterController.Move(moveDirection * Time.deltaTime * playerSpeed * speed);
            RotateCharacter(moveDirection); // ĳ���� ȸ��
        }
    }

     bool IsAttacking()
    {
        return (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") ||
                    animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") ||
                    animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3") || isAttacking);
    }

    // �̵� ���� ���
    Vector3 CalculateMoveDirection()
    {
        Vector3 lookForward = new Vector3(followCam.forward.x, 0f, followCam.forward.z).normalized;
        Vector3 lookRight = new Vector3(followCam.right.x, 0f, followCam.right.z).normalized;
        return lookForward * moveInput.y + lookRight * moveInput.x;
    }

    // ĳ���� ȸ�� ó��
    void RotateCharacter(Vector3 moveDirection)
    {
        if (moveDirection != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            characterBody.rotation = Quaternion.Slerp(characterBody.rotation, newRotation, 0.15f);
        }
    }

    // �߷� ����
    void ApplyGravity()
    {
        if (!characterController.isGrounded) velocity.y += gravity * Time.deltaTime;
        else velocity.y = -0.5f;

        characterController.Move(velocity * Time.deltaTime);
    }

    // �̵� �Է� �ڵ鷯
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // �޸��� �Է� �ڵ鷯
    void OnSprint()
    {
        isRunning = !isRunning; // �޸��� ���
    }

    // ���� �Է� �ڵ鷯
    void OnAttack()
    {
        if (characterController.isGrounded && !isDodging)
        {
            isAttacking = true;            
            animator.SetTrigger("Attack");            
        }
    }

    // ȸ�� �Է� �ڵ鷯
    void OnRoll()
    {
        if (moveInput.magnitude != 0 && !isDodging && characterController.isGrounded)
        {
            isDodging = true;
            dodgeVec = CalculateMoveDirection().normalized; // �̵� ������ ȸ�� �������� ����
            animator.SetTrigger("Dodge"); // ȸ�� �ִϸ��̼� ���
            characterController.center = new Vector3(0, 0.5f, 0); // ĳ���� �߽� ����
            characterController.height = 1f; // ĳ���� ���� ����
            // ȸ�� ���� �� ĳ���͸� ȸ�� �������� �ٷ� ȸ����Ŵ
            characterBody.rotation = Quaternion.LookRotation(dodgeVec);
        }
    }
    
    void ResetAttack()=> isAttacking = false;   

    // ȸ�� �ִϸ��̼��� ���� �� ȣ��Ǵ� �̺�Ʈ
    void EndDodge()
    {
        // ȸ�� ���� �� ĳ���� �߽ɰ� ���̸� �ʱ�ȭ�ϰ� ȸ�� �÷��׸� ��Ȱ��ȭ��
        characterController.center = new Vector3(0, 0.88f, 0);
        characterController.height = 1.6f;
        isDodging = false;
        ResetAttack(); 
    }
}