using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
    Animator animator;
    bool isColliding = false;
    CharacterController characterController;

    public float moveDistance = 1.0f; // 이동 거리 조절 가능
    public string hitTriggerName = "hitTrigger"; // 트리거 애니메이션 이름
    public float animationDuration = 0.5f; // 애니메이션 재생 시간



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

        // 트리거로 설정된 애니메이션 재생
        animator.SetTrigger(hitTriggerName);

        while (elapsed < animationDuration) {
            float t = elapsed / animationDuration;

            // 이동 벡터 계산
            Vector3 moveVector = Vector3.Lerp(startPosition, endPosition, t) - transform.position;

            // 캐릭터 컨트롤러를 사용하여 이동
            characterController.Move(moveVector);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 이동이 끝난 후에 다른 처리를 추가할 수 있습니다.

        // 이동이 끝났으므로 충돌 상태를 해제합니다.
        isColliding = false;
    }
    
}