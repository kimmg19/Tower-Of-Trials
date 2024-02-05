using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    void OnControllerColliderHit(ControllerColliderHit hit) {
        print("큐브 충돌");
        
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
}
