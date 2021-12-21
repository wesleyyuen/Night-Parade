using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MEC;

public class DialogueManager : MonoBehaviour
{
    static DialogueManager instance;
    public static DialogueManager Instance {
        get  {return instance; }
    }

    Queue<Dialogue.Sentence> sentences;
    [SerializeField] GameObject dialogueUI;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI characterNameText;
    [SerializeField] float typingSpeed;
    float _originalTypingSpeed;
    public bool isTalking { get; private set; }
    Dialogue.Sentence currentSentence;

    void Awake()
    {
        if (instance == null) {
            instance = this;
            //DontDestroyOnLoad (gameObject); // Handled by Parent
        } else {
            Destroy (gameObject);
        }
    }
    
    void Start()
    {
        sentences = new Queue<Dialogue.Sentence> ();
        currentSentence = new Dialogue.Sentence("", null);
        dialogueUI.SetActive (false);
        _originalTypingSpeed = typingSpeed;
        isTalking = false;
    }

    void Update()
    {
        if (isTalking) {
            if (Input.GetButtonDown ("Attack") || Input.GetButtonDown ("Jump")) {
                // Next sentence if entire sentence is done displaying
                if (dialogueText.text == currentSentence.sentence) {
                    NextSentence ();
                }
            }

            // Hold to changing Text speed
            if (Input.GetButton ("Attack") || Input.GetButton ("Jump")) {
                typingSpeed = _originalTypingSpeed * 3;
            } else {
                typingSpeed = _originalTypingSpeed;
            }
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isTalking = true;
        dialogueUI.SetActive (true);

        // Disable player control
        Utility.EnablePlayerControl(false);

        sentences.Clear();

        Dialogue.SentenceSet chosenSet;
        // Choose set based on random or sequential
        if (dialogue.setSelectionMode == Dialogue.SetSelectionMode.Random) {
            chosenSet = dialogue.sentenceSets[UnityEngine.Random.Range (0, dialogue.sentenceSets.Length)];
        } else {
            chosenSet = dialogue.sentenceSets[dialogue.currentSetIndex];
            if (dialogue.currentSetIndex < dialogue.sentenceSets.Length - 1) dialogue.currentSetIndex++;
        }

        // Queue up sentences in set
        foreach (Dialogue.Sentence sentence in chosenSet.sentences) {
            sentences.Enqueue (sentence);
        }

        // Set Character Name
        characterNameText.text = dialogue.characterName;

        // Get First Sentence
        NextSentence();
    }

    public void NextSentence()
    {
        // Reset Dialogue UI textbox
        dialogueText.text = "";

        // End if all sentences in set are displayed
        if (sentences.Count == 0) {
            EndDialogue ();
            return;
        }

        // Get next sentence
        currentSentence = sentences.Dequeue ();

        // Call Callback if any
        if (currentSentence.callback != null) {
            currentSentence.callback.Invoke();
        }

        // Start typing coroutine
        Timing.RunCoroutine(_TpyingEffect(currentSentence.sentence));
    }

    void EndDialogue()
    {
        // Reset + reenable player controls
        typingSpeed = _originalTypingSpeed;
        isTalking = false;
        dialogueUI.SetActive (false);
        Utility.EnablePlayerControl(true);
    }

    // IEnumerator TpyingEffect(string sentence)
    // {
    //     // DOES NOT HAVE COLOR OPTIONS
    //     foreach (char letter in sentence.ToCharArray ()) {
    //         dialogueText.text += letter;
    //         yield return new WaitForSeconds (1 / typingSpeed);
    //     }
    // }

    //TODO: fix typing effect with color, nondeterministic behaviors
    IEnumerator<float> _TpyingEffect (string currentSentence) {
        int totalVisibleCharacters = dialogueText.textInfo.characterCount;
        dialogueText.text = currentSentence;
        // int totalVisibleCharacters = currentSentence.Length;
        int counter = 0;
        while (true) {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            dialogueText.maxVisibleCharacters = visibleCount;
            if (visibleCount >= totalVisibleCharacters) {
                yield return Timing.WaitForSeconds(1f);
                // yield return null;
                break;
            }
            counter++;
            yield return Timing.WaitForSeconds(1 / typingSpeed);
        }
    }
}