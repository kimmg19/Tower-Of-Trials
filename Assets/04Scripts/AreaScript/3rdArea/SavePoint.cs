using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] public GameObject player;
    [SerializeField] public PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        LoadPlayerPosition();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어의 현재 위치 저장
            SavePlayerPosition(player.transform.position);
            Debug.Log("세이브포인트 도착");

            gameObject.SetActive(false);
        }
    }

    private void SavePlayerPosition(Vector3 position)
    {
        PlayerPrefs.SetFloat("PlayerPosX", position.x);
        PlayerPrefs.SetFloat("PlayerPosY", position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", position.z);
        PlayerPrefs.Save(); // 변경 사항을 저장
    }

    public void LoadPlayerPosition()
    {
        if (PlayerPrefs.HasKey("PlayerPosX") && PlayerPrefs.HasKey("PlayerPosY") && PlayerPrefs.HasKey("PlayerPosZ"))
        {
            float x = PlayerPrefs.GetFloat("PlayerPosX");
            float y = PlayerPrefs.GetFloat("PlayerPosY");
            float z = PlayerPrefs.GetFloat("PlayerPosZ");
            player.transform.position = new Vector3(x, y, z); // 저장된 위치로 이동
            playerMovement.VelocityNormalize();
            Debug.Log($"이동할 위치: ({x}, {y}, {z})");
            Debug.Log("리트 드가자");
        }
    }
}
