using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Teleport : MonoBehaviour
{
    [SerializeField]
    private GameObject outPortal;
    [SerializeField]
    private GameObject AskSelection;

    GameObject obj;
    PlayerInputs playerInputs;

    void Start()
    {
        obj = GameObject.Find("Player");
        playerInputs = obj.GetComponent<PlayerInputs>();

        if (AskSelection != null)
        {
            AskSelection.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInputs.isGPress && AskSelection.activeSelf)
        {
            AskSelection.SetActive(false);
            obj.SetActive(false); // 순간이동 전에 player 비활성화해야됨
            obj.transform.position = outPortal.transform.position;
            obj.SetActive(true);
            print("G키 입력");
        }
        else playerInputs.isGPress = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("빨간 포탈 충돌");
            AskSelection.SetActive(true);
        }
    }
}
