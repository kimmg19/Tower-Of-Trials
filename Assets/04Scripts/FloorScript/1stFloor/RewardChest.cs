using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RewardChest : MonoBehaviour
{
    Animator animator;
    [SerializeField]
    private GameObject AskSelection;
    [SerializeField]
    private GameObject Rewards;
    [SerializeField]
    private GameObject RewardsChest;
    GameObject obj;
    PlayerInputs playerInputs;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        obj = GameObject.Find("Player");
        playerInputs = obj.GetComponent<PlayerInputs>();
        if (AskSelection != null)
        {
            AskSelection.SetActive(false);
        }

        if(Rewards != null)
        {
            Rewards.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInputs.isGPress && AskSelection.activeSelf)
        {
            animator.SetTrigger("RewardChestOpen");
            playerInputs.isInteracting = true;
            AskSelection.SetActive(false);
            RewardsChest.SetActive(false);
            Rewards.SetActive(true);
            playerInputs.isInteracting = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            // ĵ���� Ȱ��ȭ
            AskSelection.SetActive(true);
            Debug.Log("���� �浹");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ĵ���� ��Ȱ��ȭ
            if (AskSelection != null)
            {
                AskSelection.SetActive(false);
            }
            playerInputs.isInteracting = false;

        }
    }
}
