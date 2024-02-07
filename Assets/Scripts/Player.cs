using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
    Animator animator;
    bool isColliding = false;
    CharacterController characterController;

    public float moveDistance = 1.0f; // �̵� �Ÿ� ���� ����
    public string hitTriggerName = "hitTrigger"; // Ʈ���� �ִϸ��̼� �̸�
    public float animationDuration = 0.5f; // �ִϸ��̼� ��� �ð�



    void Start() {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {

        if (hit.gameObject.CompareTag("Cube") && !isColliding) {
            isColliding = true;
            StartCoroutine(MoveCharacterWithAnimation());
        }
    }

    IEnumerator MoveCharacterWithAnimation() {
        float elapsed = 0f;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position - transform.forward * moveDistance;

        // Ʈ���ŷ� ������ �ִϸ��̼� ���
        animator.SetTrigger(hitTriggerName);

        while (elapsed < animationDuration) {
            float t = elapsed / animationDuration;

            // �̵� ���� ���
            Vector3 moveVector = Vector3.Lerp(startPosition, endPosition, t) - transform.position;

            // ĳ���� ��Ʈ�ѷ��� ����Ͽ� �̵�
            characterController.Move(moveVector);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // �̵��� ���� �Ŀ� �ٸ� ó���� �߰��� �� �ֽ��ϴ�.

        // �̵��� �������Ƿ� �浹 ���¸� �����մϴ�.
        isColliding = false;
    }
    
}