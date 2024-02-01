using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pla : MonoBehaviour {
    private Animator animator;
    private Vector3 moveDirection;
    private float moveSpeed = 4f;
    private float rotationSpeed = 10f; // ȸ�� �ӵ� ������
    Rigidbody body;
    void Start() {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();


    }

    void Update() {
        Move();
    }

    void Move() {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool hasControl = input != Vector2.zero;

        // isMoving �ִϸ��̼��� ����
        animator.SetBool("IsMoving", hasControl);

        if (hasControl) {
            // input�� �̿��Ͽ� �̵� ���� ����
            moveDirection = new Vector3(input.x, 0f, input.y);
            moveDirection.Normalize();

            // ��ǥ ȸ�� ���� ���
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            // �ε巯�� ȸ���� ���� Slerp ���
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // ĳ���� �̵�
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    public void OnMove(InputAction.CallbackContext context) {
        Vector2 input = context.ReadValue<Vector2>();
        // input.magnitude�� ���� ������ ���θ� Ȯ��
        animator.SetFloat("moveSpeed", input.magnitude);
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Plane")) {
            HandlePlaneCollision(collision);
            return;
        }

        Debug.Log("Collision with: " + collision.collider.gameObject.name);
        animator.SetTrigger("HitTrigger"); // �浹 ���·� ����
                                           // HitTrigger�� ������ �Ŀ� �������� �߰��ϰ� �ʹٸ� ���⼭ �Լ� ȣ��
        StartCoroutine(MoveCharacterDuringAnimation());
    }

    void HandlePlaneCollision(Collision collision) {
        // "Plane" ���̾ ���� ��ü���� �浹�� ����
        Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());

        // Plane���� �浹�̸鼭 Rigidbody�� �ִٸ� ���ú����̼��� �ٷ� ����
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if (rigidbody != null) {
            rigidbody.velocity = Vector3.zero; // ��ü�� �ӵ��� 0���� ����
            Debug.Log("Ignoring collision with: " + collision.collider.gameObject.name);

        }
    }

    // �ִϸ��̼� �̺�Ʈ���� ȣ���� �޼���
    IEnumerator MoveCharacterDuringAnimation() {
        float duration = 0.5f; // �� �̵� �ð�
        float elapsed = 0f;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position - transform.forward; // �ڷ� �̵�

        while (elapsed < duration) {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // �̵��� ���� �Ŀ� �ٸ� ó���� �߰��� �� �ֽ��ϴ�.
    }
}
