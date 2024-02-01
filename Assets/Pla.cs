using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pla : MonoBehaviour {
    private Animator animator;
    private Vector3 moveDirection;
    private float moveSpeed = 4f;
    private float rotationSpeed = 10f; // 회전 속도 조절값
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

        // isMoving 애니메이션을 설정
        animator.SetBool("IsMoving", hasControl);

        if (hasControl) {
            // input을 이용하여 이동 방향 설정
            moveDirection = new Vector3(input.x, 0f, input.y);
            moveDirection.Normalize();

            // 목표 회전 각도 계산
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            // 부드러운 회전을 위해 Slerp 사용
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // 캐릭터 이동
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    public void OnMove(InputAction.CallbackContext context) {
        Vector2 input = context.ReadValue<Vector2>();
        // input.magnitude를 통해 움직임 여부를 확인
        animator.SetFloat("moveSpeed", input.magnitude);
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

    // 애니메이션 이벤트에서 호출할 메서드
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
}
