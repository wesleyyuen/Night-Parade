using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [HideInInspector] public GameObject player;
    [SerializeField] bool triggerOnCollision; // trigger without pressing interact
    [SerializeField] Dialogue dialogue;
    public float triggerRange;
    public MeshRenderer textPrompt;

    void Start ()
    {
        player = GameObject.FindGameObjectWithTag ("Player");
    }

    public virtual void Update ()
    {
        bool isTalking = DialogueManager.Instance.isTalking;
        if (textPrompt == null || player == null || triggerOnCollision) return;
        
        // Control dialogue promopt text
        if (!isTalking && !triggerOnCollision && Vector2.Distance (player.transform.position, transform.position) <= triggerRange) {
            textPrompt.enabled = true;
            if (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.DownArrow)) TriggerDialogue ();
        } else {
            textPrompt.enabled = false;
        }
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (triggerOnCollision && other.CompareTag ("Player")) {
            TriggerDialogue ();
        }
    }

    public void TriggerDialogue ()
    { 
        // TODO: later maybe generalize and make it possible to pick a bool flag in inspector to choose dailogue
        if (!PauseMenu.isPuased) {
            DialogueManager.Instance.StartDialogue (dialogue);
        }
    }
}