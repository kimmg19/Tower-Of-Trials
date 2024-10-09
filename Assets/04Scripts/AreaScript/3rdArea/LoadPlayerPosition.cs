using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayerPosition : MonoBehaviour
{
    public GameObject player; // �÷��̾ ������ ����
    public PlayerStatus playerstatus;

    void Start()
    {
        player = GameObject.FindWithTag("Player"); // �÷��̾� ������Ʈ ã��
        playerstatus = player.GetComponent<PlayerStatus>(); // PlayerStatus ������Ʈ ��������
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatus playerstatus = player.GetComponent<PlayerStatus>(); // �÷��̾��� PlayerStatus ��������

            if (playerstatus != null) // null üũ
            {
                Debug.Log("�浹!");
                playerstatus.Descent(); // HP ���� �� ���̺� ����Ʈ�� �̵�
            }
            else
            {
                Debug.LogError("PlayerStatus�� ã�� �� �����ϴ�.");
            }
        }
    }
}
