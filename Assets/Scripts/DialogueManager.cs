using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour {

    private static DialogueManager Instance;

    Queue<string> sentences;
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI characterNameText;
    public float typingSpeed;
    float originalTypingSpeed;
    public bool isTalking {get; private set;}
    string currentSentence = "";

    void Awake () {
        if (Instance == null) {
            Instance = this;
            //DontDestroyOnLoad (gameObject); // Handled by Parent
        } else {
            Destroy (gameObject);
        }
    }
    void Start () {
        sentences = new Queue<string> ();
        dialogueUI.SetActive(false);
        originalTypingSpeed = typingSpeed;
        isTalking = false;
    }

    void Update () {
        if (isTalking) {
            if (Input.GetButton ("Attack") || Input.GetButton ("Jump")) {
                if (dialogueText.text == currentSentence) {
                    NextSentence ();
                } else {
                    typingSpeed = originalTypingSpeed * 3;
                }
            } else {
                typingSpeed = originalTypingSpeed;
            }
        }
    }

    public void StartDialogue (Dialogue dialogue) {
        isTalking = true;
        dialogueUI.SetActive(true);
        GameObject player = FindObjectOfType<PlayerMovement> ().gameObject;
        player.GetComponent<Animator>().SetFloat("Horizontal", 0f);
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<PlayerCombat> ().enabled = false;
        
        sentences.Clear ();
        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue (sentence);
        }
        characterNameText.text = dialogue.characterName;
        NextSentence ();
    }

    public void NextSentence () {
        dialogueText.text = "";
        if (sentences.Count == 0) {
            EndDialogue ();
            return;
        }
        currentSentence = sentences.Dequeue ();
        StartCoroutine(TpyingEffect(currentSentence));
    }

    void EndDialogue () {
        isTalking = false;
        dialogueUI.SetActive(false);
        FindObjectOfType<PlayerMovement> ().enabled = true;
        FindObjectOfType<PlayerCombat> ().enabled = true;
    }

    IEnumerator TpyingEffect (string sentence) {
        foreach (char letter in sentence.ToCharArray ()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds (1 / typingSpeed);
        }
    }
}