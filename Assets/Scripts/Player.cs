using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
    Animator animator;
    bool isColliding = false;
    CharacterController characterController;

    public float moveDistance = 1.0f; // 이동 거리 조절 변수
    public string hitTriggerName = "hitTrigger"; // 트리거 이름
    public float animationDuration = 0.5f; // 애니메이션 재생 시간

    void Start() {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        // 특정 태그의 오브젝트와 충돌하고 현재 충돌 중이 아니면
        if (hit.gameObject.CompareTag("Enemy") && !isColliding) {
            isColliding = true; // 충돌 중으로 플래그 설정
            StartCoroutine(MoveCharacterWithAnimation()); // 애니메이션과 함께 캐릭터 이동 시작
        }
    }

    IEnumerator MoveCharacterWithAnimation() {
        float elapsed = 0f;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position - transform.forward * moveDistance;

        // 충돌 트리거를 설정하여 애니메이션 시작
        animator.SetTrigger(hitTriggerName);

        while (elapsed < animationDuration) {
            float t = elapsed / animationDuration;

            // 이동 벡터 계산 및 캐릭터 이동
            Vector3 moveVector = Vector3.Lerp(startPosition, endPosition, t) - transform.position;
            characterController.Move(moveVector);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 이동이 끝나면 충돌 플래그를 해제합니다.
        isColliding = false;
    }
}
