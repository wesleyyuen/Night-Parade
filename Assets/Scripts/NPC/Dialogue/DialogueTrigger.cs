using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] bool _isAutoTrigger; // trigger without pressing interact
    [SerializeField] Dialogue dialogue;
    [SerializeField] protected float triggerRange;
    [SerializeField] protected GameObject textPrompt;
    protected GameObject _player;
    protected bool _isInRange;
    protected bool _isDialogueTriggered;


    void Start ()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        Utility.SetAlphaRecursively(textPrompt, 0f);

        // Handle Input
        InputMaster input = new InputMaster();
        input.Player.Interact.Enable();
        input.Player.Interact.started += OnTriggerDialogue;
    }

    protected virtual void Update ()
    {
        if (textPrompt == null || _player == null) return;

        bool wasInRange = _isInRange;
        _isInRange = Vector2.Distance(_player.transform.position, transform.position) <= triggerRange;

        // Trigger Text Prompt
        if (!wasInRange && _isInRange) {
            Utility.FadeGameObjectRecursively(textPrompt, 0f, 1f, 0.25f);
            if (!_isDialogueTriggered && _isAutoTrigger)
                TriggerDialogue();

        } else if (wasInRange && !_isInRange) {
            Utility.FadeGameObjectRecursively(textPrompt, 1f, 0f, 0.25f);
            _isDialogueTriggered = false;
        }
    }

    protected virtual void OnTriggerDialogue(InputAction.CallbackContext context)
    {
        if (_isInRange && !DialogueManager.Instance.isTalking && !_isAutoTrigger)
           TriggerDialogue();
    }

    public void TriggerDialogue ()
    { 
        // TODO: later maybe generalize and make it possible to pick a bool flag in inspector to choose dailogue
        if (!PauseMenu.isPuased) {
            _isDialogueTriggered = true;
            DialogueManager.Instance.StartDialogue(dialogue);
        }
    }
}