﻿using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] bool _isAutoTrigger; // trigger without pressing interact
    [SerializeField] Dialogue dialogue;
    [SerializeField] protected float triggerRange;
    [SerializeField] protected Animator textPrompt;
    protected GameObject _player;
    protected bool _isInRange;
    protected bool _isDialogueTriggered;


    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        InputManager.Event_Input_Interact += OnTriggerDialogue;
    }

    private void OnDisable()
    {
        InputManager.Event_Input_Interact -= OnTriggerDialogue;
    }

    protected virtual void Update()
    {
        if (textPrompt == null || _player == null) return;

        bool wasInRange = _isInRange;
        _isInRange = Vector2.Distance(_player.transform.position, transform.position) <= triggerRange;

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