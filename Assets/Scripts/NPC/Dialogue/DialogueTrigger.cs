using UnityEngine;

public class DialogueTrigger : MonoBehaviour {
    public GameObject player;
    public Dialogue dialogue;
    public float triggerRange;

    void Start () {
        player = GameObject.FindGameObjectWithTag ("Player");
    }
    public virtual void Update() {
        bool isTalking = FindObjectOfType<DialogueManager>().isTalking;
        if (!isTalking && Vector2.Distance(player.transform.position, transform.position) <= triggerRange && Input.GetKeyDown(KeyCode.DownArrow)) {
            TriggerDialogue();
        }
    }

    public void TriggerDialogue() {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}