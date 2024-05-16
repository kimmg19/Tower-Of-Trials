using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Teleport : MonoBehaviour
{
    PlayerInputs playerInputs;
    [SerializeField]
    private GameObject outPortal;
    [SerializeField]
    private GameObject AskSelection;

    GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        obj = GameObject.Find("Player");
        playerInputs = obj.GetComponent<PlayerInputs>();

        if (AskSelection != null)
        {
            AskSelection.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInputs.isGPress && AskSelection.activeSelf)
        {
            AskSelection.SetActive(false);
            obj.SetActive(false); // �����̵� ���� player ��Ȱ��ȭ�ؾߵ�
            obj.transform.position = outPortal.transform.position;
            obj.SetActive(true);
            print("GŰ �Է�");
        }
        else playerInputs.isGPress = false;
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
