using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayerPosition : MonoBehaviour
{
    public GameObject player; // 플레이어를 참조할 변수
    public PlayerStatus playerstatus;

    void Start()
    {
        player = GameObject.FindWithTag("Player"); // 플레이어 오브젝트 찾기
        playerstatus = player.GetComponent<PlayerStatus>(); // PlayerStatus 컴포넌트 가져오기
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatus playerstatus = player.GetComponent<PlayerStatus>(); // 플레이어의 PlayerStatus 가져오기

            if (playerstatus != null) // null 체크
            {
                Debug.Log("충돌!");
                playerstatus.Descent(); // HP 감소 및 세이브 포인트로 이동
            }
            else
            {
                Debug.LogError("PlayerStatus를 찾을 수 없습니다.");
            }
        }
    }
}
