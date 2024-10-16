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
        // �÷��̾���� ���� ���� ��� (Y�� ����)
        Vector3 direction = player.position - gameObject.transform.position;
        direction.y = 0; // Y���� 0���� �����Ͽ� ���� ���⸸ ���

        // ���� ���͸� �������� ȸ�� ����
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            gameObject.transform.rotation = targetRotation;
        }
    }
}
