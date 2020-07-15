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
                // Next sentence if entire sentence is done displaying
                if (dialogueText.text == currentSentence) {
                    NextSentence ();
                }
            }

            // Hold to changing Text speed
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

        // Disable player control
        GameObject player = FindObjectOfType<PlayerMovement> ().gameObject;
        player.GetComponent<Animator> ().SetFloat ("Horizontal", 0f);
        player.GetComponent<PlayerMovement> ().enabled = false;
        player.GetComponent<PlayerCombat> ().enabled = false;

        sentences.Clear ();

        Dialogue.SentenceSet chosenSet;
        // Choose set based on random or sequential
        if (dialogue.setSelectionMode == Dialogue.SetSelectionMode.Random) {
            chosenSet = dialogue.sentenceSets[UnityEngine.Random.Range (0, dialogue.sentenceSets.Length)];
        } else {
            chosenSet = dialogue.sentenceSets[dialogue.currentSetIndex];
            if (dialogue.currentSetIndex < dialogue.sentenceSets.Length - 1) dialogue.currentSetIndex++;
        }

        // Queue up sentences in set
        foreach (string sentence in chosenSet.sentences) {
            sentences.Enqueue (sentence);
        }

        // Set Character Name
        characterNameText.text = dialogue.characterName;

        // Get First Sentence
        NextSentence ();
    }

    public void NextSentence () {
        // Reset Dialogue UI textbox
        dialogueText.text = "";

        // End if all sentences in set are displayed
        if (sentences.Count == 0) {
            EndDialogue ();
            return;
        }

        // Get next sentence
        currentSentence = sentences.Dequeue ();

        // Start typing coroutine
        StartCoroutine (TpyingEffect (currentSentence));
    }

    void EndDialogue () {
        // Reset + reenable player controls
        typingSpeed = originalTypingSpeed;
        isTalking = false;
        dialogueUI.SetActive (false);
        FindObjectOfType<PlayerMovement> ().enabled = true;
        FindObjectOfType<PlayerCombat> ().enabled = true;
    }

    IEnumerator TpyingEffect (string sentence) {
        // DOES NOT HAVE COLOR OPTIONS
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