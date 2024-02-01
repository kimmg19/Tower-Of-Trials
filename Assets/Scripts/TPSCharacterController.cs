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
        animator.SetTrigger("HitTrigger"); // 충돌 상태로 변경
                                           // HitTrigger를 실행한 후에 움직임을 추가하고 싶다면 여기서 함수 호출
        StartCoroutine(MoveCharacterDuringAnimation());
    }

    void HandlePlaneCollision(Collision collision) {
        // "Plane" 레이어를 가진 물체와의 충돌을 무시
        Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());

        // Plane과의 충돌이면서 Rigidbody가 있다면 데시벨레이션을 바로 적용
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if (rigidbody != null) {
            rigidbody.velocity = Vector3.zero; // 물체의 속도를 0으로 만듦
            Debug.Log("Ignoring collision with: " + collision.collider.gameObject.name);

        }
    }
    IEnumerator MoveCharacterDuringAnimation() {
        float duration = 0.5f; // 총 이동 시간
        float elapsed = 0f;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position - transform.forward; // 뒤로 이동

        while (elapsed < duration) {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 이동이 끝난 후에 다른 처리를 추가할 수 있습니다.
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
