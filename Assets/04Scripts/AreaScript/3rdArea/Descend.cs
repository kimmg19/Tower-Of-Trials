using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Descend : MonoBehaviour
{
    PlayerStatus playerstatus;
    GameObject player;
    bool hasDescentTriggered = false; 
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
            playerstatus.Descent(); // HP ���� �� ���̺� ����Ʈ�� �̵�
        }
    }

    public void ResetDescentTrigger()
    {
        hasDescentTriggered = false;
    }
}
