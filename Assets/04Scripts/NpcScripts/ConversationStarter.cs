using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class ConversationStarter : MonoBehaviour
{
    [SerializeField]
    private GameObject interactionPrompt; // G키 안내 UI
    GameObject player;
    PlayerInputs playerInputs;
    [SerializeField] private GameObject conversation;
    [SerializeField] private NPCConversation myConversation;
    private bool isPlayerInRange = false;
    Animator playerAnimator; // 플레이어 애니메이터를 추가합니다.

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerInputs = player.GetComponent<PlayerInputs>();
        playerAnimator = player.GetComponent<Animator>(); // 플레이어 애니메이터 참조

        // 초기화 로그 추가
        Debug.Log("NPCInteraction Awake: Initialized player and playerInputs.");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            interactionPrompt.SetActive(true);
            conversation.SetActive(true);
            isPlayerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            interactionPrompt.SetActive(false);
            conversation.SetActive(false);
            isPlayerInRange = false;
        }
    }

    void Update()
    {
        if (isPlayerInRange && playerInputs.isGPress)
        {
            // 대화를 시작
            ConversationManager.Instance.StartConversation(myConversation);
            interactionPrompt.SetActive(false); // 안내 문구 종료
            conversation.SetActive(true);
            playerInputs.isGPress = false;
            
            // 플레이어 애니메이션 속도 설정
            playerAnimator.SetFloat("speed", 0);

            // 플레이어 상호작용 상태로 설정
            playerInputs.isInteracting = true;

            // 대화 종료 이벤트 핸들러 설정
            ConversationManager.OnConversationEnded += EndConversation;
        }
    }

    public void EndConversation()
    {
        conversation.SetActive(false);

        // 대화 종료 시 애니메이션 속도를 원래대로 복구
        playerAnimator.SetFloat("speed", playerInputs.moveInput.magnitude); // 원래 속도로 설정
        playerInputs.isInteracting = false;

        // 대화 종료 이벤트 핸들러 해제
        ConversationManager.OnConversationEnded -= EndConversation;
    }
}