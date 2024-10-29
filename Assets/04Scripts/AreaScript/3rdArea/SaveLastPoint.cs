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
            // �÷��̾��� ���� ��ġ ����
            SaveResetPosition(player.transform.position);

            gameObject.SetActive(false);
        }
    }

    private void SaveResetPosition(Vector3 position)
    {
        PlayerPrefs.SetFloat("PlayerPosX", 0);
        PlayerPrefs.SetFloat("PlayerPosY", 0);
        PlayerPrefs.SetFloat("PlayerPosZ", 0);
        PlayerPrefs.Save(); // ���� ������ ����
        float x = PlayerPrefs.GetFloat("PlayerPosX");
        float y = PlayerPrefs.GetFloat("PlayerPosY");
        float z = PlayerPrefs.GetFloat("PlayerPosZ");
    }
}
