using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Descend : MonoBehaviour
{
    PlayerStatus playerstatus;
    GameObject player;
    bool hasDescentTriggered = false; // �̹� HP ���ҿ� ��ġ �̵��� �Ǿ����� ���θ� Ȯ��
    [SerializeField] PlayerInputs playerInputs;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerstatus = player.GetComponent<PlayerStatus>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasDescentTriggered)
        {
            Debug.Log("�浹!");
            playerstatus.Descent(); // HP ���� �� ���̺� ����Ʈ�� �̵�
            //hasDescentTriggered = true; // �� ���� ȣ��ǵ��� ����
        }
    }

    public void ResetDescentTrigger()
    {
        hasDescentTriggered = false;
    }
}
