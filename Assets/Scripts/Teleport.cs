using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Teleport : MonoBehaviour
{
    [SerializeField]
    private GameObject outPortal;
    [SerializeField]
    private GameObject AskSelection;

    GameObject obj;
    PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        obj = GameObject.Find("Player");
        playerMovement = obj.GetComponent<PlayerMovement>();

        if (AskSelection != null)
        {
            AskSelection.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.isGPress && AskSelection.activeSelf)
        {
            AskSelection.SetActive(false);
            obj.SetActive(false); // �����̵� ���� player ��Ȱ��ȭ�ؾߵ�
            obj.transform.position = outPortal.transform.position;
            obj.SetActive(true);
            print("GŰ �Է�");
        }
        else playerMovement.isGPress = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("���� ��Ż �浹");
            AskSelection.SetActive(true);
        }
    }
}
