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
    Animator animaor;


    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerInputs = player.GetComponent<PlayerInputs>();
        animaor = GetComponent<Animator>();


        // 초기화 로그 추가
        Debug.Log("NPCInteraction Awake: Initialized player and playerInputs.");

        // 프롬프트를 초기에는 비활성화

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
            // isGPress를 초기화
            playerInputs.isGPress = false;

            // 플레이어 상호작용 상태로 설정
            playerInputs.isInteracting = true;

            // 대화 종료 이벤트 핸들러 설정
            ConversationManager.OnConversationEnded += EndConversation;
        }
    }

    // 대화 종료 시 호출되는 함수
    public void EndConversation()
    {
        
        conversation.SetActive(false);

        // 플레이어 상호작용 상태 해제
        playerInputs.isInteracting = false;

        // 대화 종료 이벤트 핸들러 해제
        ConversationManager.OnConversationEnded -= EndConversation;
    }
}
