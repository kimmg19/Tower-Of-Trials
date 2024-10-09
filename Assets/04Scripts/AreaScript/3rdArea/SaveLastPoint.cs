using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLastPoint : MonoBehaviour
{
    [SerializeField] public GameObject player;
    [SerializeField] public PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어의 현재 위치 저장
            SaveResetPosition(player.transform.position);
            Debug.Log("세이브포인트 리셋");

            gameObject.SetActive(false);
        }
    }

    private void SaveResetPosition(Vector3 position)
    {
        PlayerPrefs.SetFloat("PlayerPosX", 0);
        PlayerPrefs.SetFloat("PlayerPosY", 0);
        PlayerPrefs.SetFloat("PlayerPosZ", 0);
        PlayerPrefs.Save(); // 변경 사항을 저장
        float x = PlayerPrefs.GetFloat("PlayerPosX");
        float y = PlayerPrefs.GetFloat("PlayerPosY");
        float z = PlayerPrefs.GetFloat("PlayerPosZ");
        Debug.Log($"이동할 위치: ({x}, {y}, {z})");
    }
}
