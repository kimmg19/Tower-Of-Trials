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
            // �÷��̾��� ���� ��ġ ����
            SavePlayerPosition(player.transform.position);
            Debug.Log("���̺�����Ʈ ����");

            gameObject.SetActive(false);
        }
    }

    private void SavePlayerPosition(Vector3 position)
    {
        PlayerPrefs.SetFloat("PlayerPosX", position.x);
        PlayerPrefs.SetFloat("PlayerPosY", position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", position.z);
        PlayerPrefs.Save(); // ���� ������ ����
    }

    public void LoadPlayerPosition()
    {
        if (PlayerPrefs.HasKey("PlayerPosX") && PlayerPrefs.HasKey("PlayerPosY") && PlayerPrefs.HasKey("PlayerPosZ"))
        {
            float x = PlayerPrefs.GetFloat("PlayerPosX");
            float y = PlayerPrefs.GetFloat("PlayerPosY");
            float z = PlayerPrefs.GetFloat("PlayerPosZ");
            player.transform.position = new Vector3(x, y, z); // ����� ��ġ�� �̵�
            playerMovement.VelocityNormalize();
            Debug.Log($"�̵��� ��ġ: ({x}, {y}, {z})");
            Debug.Log("��Ʈ �尡��");
        }
    }
}
