using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Teleport : MonoBehaviour
{
    [SerializeField]
    private GameObject outPortal;
    [SerializeField]
    private GameObject TeleportAskSelection;

    GameObject obj;
    PlayerInputs playerInputs;

    void Start()
    {
        obj = GameObject.Find("Player");
        playerInputs = obj.GetComponent<PlayerInputs>();

        if (TeleportAskSelection != null)
        {
            TeleportAskSelection.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInputs.isGPress && TeleportAskSelection.activeSelf)
        {
            TeleportAskSelection.SetActive(false);
            obj.SetActive(false); // 순간이동 전에 player 비활성화해야됨
            obj.transform.position = outPortal.transform.position;
            obj.SetActive(true);
        }
        else
        {

        }

    }

    void OnTriggerEnter(Collider other)
    {
        playerInputs.isGPress = false;

        if (other.CompareTag("Player"))
        {
            TeleportAskSelection.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 캔버스 비활성화
            if (TeleportAskSelection != null)
            {
                TeleportAskSelection.SetActive(false);
            }
            playerInputs.isGPress = false;
        }
    }
}
