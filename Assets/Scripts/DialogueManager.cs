using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour {

    private static DialogueManager Instance;

    private Queue<string> sentences;
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private float typingSpeed;
    private float originalTypingSpeed;
    public bool isTalking { get; private set; }
    private string currentSentence = "";

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
        dialogueUI.SetActive (false);
        originalTypingSpeed = typingSpeed;
        isTalking = false;
    }

    void Update () {
        if (isTalking) {
            if (Input.GetButtonDown ("Attack") || Input.GetButtonDown ("Jump")) {
                if (dialogueText.text == currentSentence) {
                    NextSentence ();
                }
            }
            if (Input.GetButton ("Attack") || Input.GetButton ("Jump")) {
                typingSpeed = originalTypingSpeed * 3;
            } else {
                typingSpeed = originalTypingSpeed;
            }
        }
    }

    public void StartDialogue (Dialogue dialogue) {
        isTalking = true;
        dialogueUI.SetActive (true);
        GameObject player = FindObjectOfType<PlayerMovement> ().gameObject;
        player.GetComponent<Animator> ().SetFloat ("Horizontal", 0f);
        player.GetComponent<PlayerMovement> ().enabled = false;
        player.GetComponent<PlayerCombat> ().enabled = false;

        sentences.Clear ();

        Dialogue.SentenceSet chosenSet;
        if (dialogue.setSelectionMode == Dialogue.SetSelectionMode.Random) {
            chosenSet = dialogue.sentenceSets[UnityEngine.Random.Range (0, dialogue.sentenceSets.Length)];
        } else { // sentence set chosen sequentially
            chosenSet = dialogue.sentenceSets[dialogue.currentSetIndex];
            if (dialogue.currentSetIndex < dialogue.sentenceSets.Length - 1) dialogue.currentSetIndex++;
        }
        foreach (string sentence in chosenSet.sentences) {
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
        StartCoroutine (TpyingEffect (currentSentence));
    }

    void EndDialogue () {
        typingSpeed = originalTypingSpeed;
        isTalking = false;
        dialogueUI.SetActive (false);
        FindObjectOfType<PlayerMovement> ().enabled = true;
        FindObjectOfType<PlayerCombat> ().enabled = true;
    }

    IEnumerator TpyingEffect (string sentence) {
        foreach (char letter in sentence.ToCharArray ()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds (1 / typingSpeed);
        }
    }

    /*
    //TODO: fix typing effect with color, nondeterministic behaviors
    IEnumerator TpyingEffect (string currentSentence) {
        //int totalVisibleCharacters = dialogueText.textInfo.characterCount;
        dialogueText.text = currentSentence;
        int totalVisibleCharacters = currentSentence.Length;
        int counter = 0;
        while (true) {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            dialogueText.maxVisibleCharacters = visibleCount;
            if (visibleCount >= totalVisibleCharacters) {
                yield return null;
                break;
            }
            counter++;
            yield return new WaitForSeconds (1 / typingSpeed);
        }
    }
    */
}