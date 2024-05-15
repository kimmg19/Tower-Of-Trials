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
            obj.SetActive(false); // 순간이동 전에 player 비활성화해야됨
            obj.transform.position = outPortal.transform.position;
            obj.SetActive(true);
            print("G키 입력");
        }
        else playerMovement.isGPress = false;
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
