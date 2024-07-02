using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSentence : MonoBehaviour {
    public string[] sentences;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Player entered NPC interaction zone.");
            DialogueManager.instance.ShowInteractionPrompt(true);
            DialogueManager.instance.SetPlayerInRange(true);
            DialogueManager.instance.sentences.Clear();
            foreach (var sentence in sentences) {
                DialogueManager.instance.sentences.Enqueue(sentence);
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Player exited NPC interaction zone.");
            DialogueManager.instance.ShowInteractionPrompt(false);
            DialogueManager.instance.SetPlayerInRange(false);
        }
    }
}
