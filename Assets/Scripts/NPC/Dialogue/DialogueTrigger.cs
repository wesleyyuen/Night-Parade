using UnityEngine;
using Zenject;

public class DialogueTrigger : MonoBehaviour
{
    private InputManager _inputManager;
    [SerializeField] private bool _isAutoTrigger;
    [SerializeField] private Dialogue dialogue;
    [SerializeField] protected Optional<float> _triggerRange;
    [SerializeField] protected Animator textPrompt;
    protected GameObject _player;
    protected bool _isInRange;
    protected bool _isDialogueTriggered;

    [Inject]
    public void Initialize(InputManager inputManager)
    {
        _inputManager = inputManager;
    }

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        _inputManager.Event_GameplayInput_Interact += OnTriggerDialogue;
    }

    private void OnDisable()
    {
        _inputManager.Event_GameplayInput_Interact -= OnTriggerDialogue;
    }

    protected virtual void Update()
    {
        if (textPrompt == null || _player == null) return;

        bool wasInRange = _isInRange;
        _isInRange = _triggerRange.Enabled ? Vector2.Distance(_player.transform.position, transform.position) <= _triggerRange.Value : true;

        // Trigger Text Prompt
        if (!wasInRange && _isInRange) {
            textPrompt.SetTrigger("Open");
            if (!_isDialogueTriggered && _isAutoTrigger)
                TriggerDialogue();

        } else if (wasInRange && !_isInRange) {
            textPrompt.SetTrigger("Close");
            _isDialogueTriggered = false;
        }
    }

    protected virtual void OnTriggerDialogue()
    {
        if (_isInRange && !DialogueManager.Instance.isTalking && !_isAutoTrigger)
           TriggerDialogue();
    }

    private void TriggerDialogue()
    { 
        // TODO: later maybe generalize and make it possible to pick a bool flag in inspector to choose dailogue
        if (!PauseMenu.isPuased) {
            _isDialogueTriggered = true;
            DialogueManager.Instance.StartDialogue(dialogue);
        }
    }
}