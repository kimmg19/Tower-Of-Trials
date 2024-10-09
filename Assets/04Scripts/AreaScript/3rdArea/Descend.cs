using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Descend : MonoBehaviour
{
    PlayerStatus playerstatus;
    GameObject player;
    bool hasDescentTriggered = false; // 이미 HP 감소와 위치 이동이 되었는지 여부를 확인
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
            Debug.Log("충돌!");
            playerstatus.Descent(); // HP 감소 및 세이브 포인트로 이동
            //hasDescentTriggered = true; // 한 번만 호출되도록 설정
        }
    }

    public void ResetDescentTrigger()
    {
        hasDescentTriggered = false;
    }
}
