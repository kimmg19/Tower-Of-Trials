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

    // 포탈 오브젝트 추가
    [SerializeField]
    private GameObject comebackPortal;
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
        // 포탈 비활성화 (처음에는 보이지 않음)
        if (comebackPortal != null)
        {
            comebackPortal.SetActive(false);
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
                AudioManager.instance.Play("OpenChest");
                RewardList.SetActive(true);

                // 포탈을 활성화
                if (comebackPortal != null)
                {
                    comebackPortal.SetActive(true);
                }
        }

        RewardGoldText.text = RewardGold.ToString() + " G";
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AskRewardSelection.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (AskRewardSelection != null)
            {
                AskRewardSelection.SetActive(false);
            }
            playerInputs.isInteracting = false;
        }
    }
}
