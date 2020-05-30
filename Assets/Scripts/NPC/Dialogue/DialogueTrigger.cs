using UnityEngine;

public class DialogueTrigger : MonoBehaviour {
    Transform player;
    public Dialogue dialogue;
    public float triggerRange;

    void Start () {
        player = GameObject.FindGameObjectWithTag ("Player").transform;
    }
    void Update() {
        bool isTalking = FindObjectOfType<DialogueManager>().isTalking;
        if (!isTalking && Vector2.Distance(player.position, transform.position) <= triggerRange && Input.GetKeyDown(KeyCode.DownArrow)) {
            TriggerDialogue();
        }
    }

    void TriggerDialogue() {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}