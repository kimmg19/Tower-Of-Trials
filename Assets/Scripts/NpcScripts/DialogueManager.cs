using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour {
    public TextMeshProUGUI npcText;
    public GameObject nextText;
    public CanvasGroup dialogueGroup;
    public GameObject interactionPrompt; // G키 안내 UI
    public Queue<string> sentences;
    private string currentSentence;
    private float typingSpeed = 0.05f;
    private bool isTyping;
    private bool playerInRange;
    private bool isDialogueActive;

    public static DialogueManager instance;
    private PlayerInputs playerInputs;

    private void Awake() {
        instance = this;
    }

    void Start() {
        sentences = new Queue<string>();
        playerInputs = GameObject.FindWithTag("Player").GetComponent<PlayerInputs>();
        interactionPrompt.SetActive(false); // G키 안내 UI 초기 비활성화
        dialogueGroup.alpha = 0; // 대화창 초기 비활성화
        dialogueGroup.blocksRaycasts = false;
    }

    public void OnDialogue(string[] lines) {
        Debug.Log("OnDialogue called");
        sentences.Clear();
        foreach (string line in lines) {
            sentences.Enqueue(line);
        }
        dialogueGroup.alpha = 1;
        dialogueGroup.blocksRaycasts = true;
        interactionPrompt.SetActive(false); // G키 안내 UI 비활성화
        isDialogueActive = true;
        NextSentence();
    }

    public void NextSentence() {
        if (sentences.Count != 0) {
            currentSentence = sentences.Dequeue();
            isTyping = true;
            nextText.SetActive(false);
            StartCoroutine(Typing(currentSentence));
        } else {
            EndDialogue();
        }
    }

    IEnumerator Typing(string line) {
        npcText.text = "";
        foreach (char letter in line.ToCharArray()) {
            npcText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void Update() {
    if (npcText.text.Equals(currentSentence)) {
        if (nextText != null) {
            nextText.SetActive(true);
        } else {
            Debug.LogError("nextText is not assigned in the Inspector.");
        }
    }

    // G키 입력을 통해 대화를 시작
    if (playerInRange && playerInputs.isGPress && !isDialogueActive) {
        Debug.Log("G key pressed to start dialogue.");
        OnDialogue(sentences.ToArray());
        playerInputs.isGPress = false;  // G키 입력 상태를 초기화
    }

    // G키 입력을 통해 다음 문장으로 넘어가기
    if (playerInputs.isGPress && !isTyping && isDialogueActive) {
        Debug.Log("G key pressed for NextSentence.");
        NextSentence();
        playerInputs.isGPress = false;  // G키 입력 상태를 초기화
    }
    }

    private void EndDialogue() {
        dialogueGroup.alpha = 0;
        dialogueGroup.blocksRaycasts = false;
        isDialogueActive = false;
    }

    public void ShowInteractionPrompt(bool show) {
        interactionPrompt.SetActive(show);
    }

    public void SetPlayerInRange(bool inRange) {
        playerInRange = inRange;
    }
}
