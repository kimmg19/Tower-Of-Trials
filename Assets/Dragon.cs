using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        LookAtPlayerXZ(player.transform);
    }
    void LookAtPlayerXZ(Transform player)
    {
        // 플레이어와의 방향 벡터 계산 (Y축 고정)
        Vector3 direction = player.position - gameObject.transform.position;
        direction.y = 0; // Y축을 0으로 고정하여 수평 방향만 고려

        // 방향 벡터를 기준으로 회전 설정
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            gameObject.transform.rotation = targetRotation;
        }
    }
}
