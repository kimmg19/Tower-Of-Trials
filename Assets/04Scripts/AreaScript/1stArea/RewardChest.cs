using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardChest : MonoBehaviour
{
    Animator animator;
    [SerializeField]
    private GameObject AskRewardSelection;
    [SerializeField]
    private GameObject RewardsChest;
    [SerializeField]
    private GameObject RewardList;

    GameObject obj;
    PlayerInputs playerInputs;

    public int RewardGold;
    public Text RewardGoldText;

    void Start()
    {
        animator = GetComponent<Animator>();
        obj = GameObject.Find("Player");
        playerInputs = obj.GetComponent<PlayerInputs>();

        if (AskRewardSelection != null)
        {
            AskRewardSelection.SetActive(false);
        }

        if (RewardList != null)
        {
            RewardList.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInputs.isGPress && AskRewardSelection.activeSelf)
        {
            animator.SetTrigger("RewardChestOpen");
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("End"))
        {
                playerInputs.isInteracting = true;
                AskRewardSelection.SetActive(false);
                RewardsChest.SetActive(false);
                playerInputs.isInteracting = false;
                RewardList.SetActive(true);
        }

        RewardGoldText.text = RewardGold.ToString() + " G";
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 캔버스 활성화
            AskRewardSelection.SetActive(true);
            Debug.Log("상자 충돌");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 캔버스 비활성화
            if (AskRewardSelection != null)
            {
                AskRewardSelection.SetActive(false);
            }
            playerInputs.isInteracting = false;
        }
    }
}
