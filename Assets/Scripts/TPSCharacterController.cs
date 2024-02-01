using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TPSCharacterController : MonoBehaviour {
    [SerializeField]
    Transform characterBody;
    [SerializeField]
    Transform cameraArm;

    Animator animator;
    void Start() {
        animator = characterBody.GetComponent<Animator>();
    }

    void Update() {
        LookAround();
        Move();
        Attack();
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

    void Attack() {
        if (Input.GetMouseButtonDown(0)) {
            animator.SetTrigger("onWeaponAttack");

        }
    }
    void Move() {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isMoving = moveInput.magnitude != 0;
        animator.SetBool("isMoving", isMoving);
        if (isMoving) {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;
            characterBody.forward = moveDir;
            transform.position += moveDir * Time.deltaTime * 8f;
        }
    }

    void LookAround() {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;
        if (x < 180f) {
            x = Mathf.Clamp(x, -1f, 70f);
        } else {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }
}
