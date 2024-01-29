using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float RotationSpeed = 5f;
    public float deceleration = 5f; // 감속도
    Rigidbody body;
    Animator animator;
    Vector3 movement;

    void Awake()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Move(h, v);
        Turn(h, v);
    }

    void Move(float h, float v)
    {
        movement.Set(h, 0, v);
        if (h == 0 && v == 0)
        {
            animator.SetBool("IsMoving", false);
        }
        else
        {
            animator.SetBool("IsMoving", true);
        }
        movement = movement.normalized * moveSpeed * Time.deltaTime;

        body.MovePosition(transform.position + movement);
    }

    void Turn(float h, float v)
    {
        if (h == 0 && v == 0)
        {
            return;
        }
        Quaternion Rotation = Quaternion.LookRotation(movement);
        body.rotation = Quaternion.Slerp(body.rotation, Rotation, RotationSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Plane"))
        {
            HandlePlaneCollision(collision);
            return;
        }

        Debug.Log("Collision with: " + collision.collider.gameObject.name);
        animator.SetTrigger("HitTrigger"); // 충돌 상태로 변경
                                           // HitTrigger를 실행한 후에 움직임을 추가하고 싶다면 여기서 함수 호출
        StartCoroutine(MoveCharacterDuringAnimation());
    }

    void HandlePlaneCollision(Collision collision)
    {
        // "Plane" 레이어를 가진 물체와의 충돌을 무시
        Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());

        // Plane과의 충돌이면서 Rigidbody가 있다면 데시벨레이션을 바로 적용
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.velocity = Vector3.zero; // 물체의 속도를 0으로 만듦
            Debug.Log("Ignoring collision with: " + collision.collider.gameObject.name);

        }
    }

    // 애니메이션 이벤트에서 호출할 메서드
    IEnumerator MoveCharacterDuringAnimation()
    {
        float duration = 0.5f; // 총 이동 시간
        float elapsed = 0f;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position - transform.forward; // 뒤로 이동

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 이동이 끝난 후에 다른 처리를 추가할 수 있습니다.
    }
}
