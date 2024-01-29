using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float RotationSpeed = 5f;
    public float deceleration = 5f; // ���ӵ�
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
        animator.SetTrigger("HitTrigger"); // �浹 ���·� ����
                                           // HitTrigger�� ������ �Ŀ� �������� �߰��ϰ� �ʹٸ� ���⼭ �Լ� ȣ��
        StartCoroutine(MoveCharacterDuringAnimation());
    }

    void HandlePlaneCollision(Collision collision)
    {
        // "Plane" ���̾ ���� ��ü���� �浹�� ����
        Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());

        // Plane���� �浹�̸鼭 Rigidbody�� �ִٸ� ���ú����̼��� �ٷ� ����
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.velocity = Vector3.zero; // ��ü�� �ӵ��� 0���� ����
            Debug.Log("Ignoring collision with: " + collision.collider.gameObject.name);

        }
    }

    // �ִϸ��̼� �̺�Ʈ���� ȣ���� �޼���
    IEnumerator MoveCharacterDuringAnimation()
    {
        float duration = 0.5f; // �� �̵� �ð�
        float elapsed = 0f;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position - transform.forward; // �ڷ� �̵�

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // �̵��� ���� �Ŀ� �ٸ� ó���� �߰��� �� �ֽ��ϴ�.
    }
}
