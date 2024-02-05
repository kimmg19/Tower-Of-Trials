using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    void OnControllerColliderHit(ControllerColliderHit hit) {
        print("ť�� �浹");
        
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
}
