using UnityEngine;
using TMPro; // TextMeshPro 네임스페이스 추가

public class NPCInteraction : MonoBehaviour
{
    [SerializeField]
    private GameObject interactionPrompt; // G키 안내 UI
    [SerializeField]
    private GameObject dialogueUI; // 대화 UI
    [SerializeField]
    private TextMeshProUGUI npcText; // TextMeshProUGUI 컴포넌트 사용
    [SerializeField]
    [TextArea]
    private string[] dialogues; // 여러 대화를 저장할 배열
    private int currentDialogueIndex = 0; // 현재 대화 인덱스

    GameObject player;
    PlayerInputs playerInputs;
    private bool isGPressLocal = false; // 로컬 G키 상태

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerInputs = player.GetComponent<PlayerInputs>();

        // 초기화 로그 추가
        Debug.Log("NPCInteraction Awake: Initialized player and playerInputs.");
    }

    void Start()
    {
        // dialogues 배열 초기화 확인
        if (dialogues == null || dialogues.Length == 0)
        {
            Debug.LogError("Dialogues array is not initialized or empty in NPCInteraction script.");
            return;
        }

        // 초기 상태 로그 출력
        Debug.Log("NPCInteraction initialized.");

        // NPC 텍스트 컴포넌트 확인
        if (npcText == null)
        {
            Debug.LogError("NPC Text component is not assigned.");
        }

        // 대화 UI 초기 상태 확인
        if (dialogueUI == null)
        {
            Debug.LogError("Dialogue UI is not assigned.");
        }
        else
        {
            dialogueUI.SetActive(false);
        }

        // 인터랙션 프롬프트 초기 상태 확인
        if (interactionPrompt == null)
        {
            Debug.LogError("Interaction prompt UI is not assigned.");
        }
        else
        {
            interactionPrompt.SetActive(false);
        }
    }

    void Update()
    {
        // G키 입력 로그 출력
        if (playerInputs.isGPress)
        {
            Debug.Log("G key pressed.");
            isGPressLocal = true;
            playerInputs.isGPress = false;
        }

        // 대화 진행 로그 출력
        if (isGPressLocal && interactionPrompt.activeSelf)
        {
            Debug.Log("Interaction prompt is active. Current dialogue index: " + currentDialogueIndex);
            if (currentDialogueIndex < dialogues.Length)
            {
                npcText.text = dialogues[currentDialogueIndex];
                dialogueUI.SetActive(true);
                interactionPrompt.SetActive(false);
                Debug.Log("Displaying dialogue: " + dialogues[currentDialogueIndex]);
                currentDialogueIndex++;
            }
            else
            {
                EndDialogue();
                Debug.Log("Ending dialogue.");
            }
            isGPressLocal = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionPrompt.SetActive(true);
            currentDialogueIndex = 0; // 대화 인덱스 초기화
            isGPressLocal = false;
            Debug.Log("Player entered NPC interaction zone.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionPrompt.SetActive(false);
            dialogueUI.SetActive(false);
            isGPressLocal = false;
            Debug.Log("Player exited NPC interaction zone.");
        }
    }

    public void OnCloseDialogue()
    {
        EndDialogue();
    }

    private void EndDialogue()
    {
        dialogueUI.SetActive(false);
        interactionPrompt.SetActive(true);
        currentDialogueIndex = 0; // 대화 인덱스 초기화
        Debug.Log("Dialogue ended.");
    }
}
