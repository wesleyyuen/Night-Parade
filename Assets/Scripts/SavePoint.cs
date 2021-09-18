using UnityEngine;
using UnityEngine.InputSystem;

public class SavePoint : DialogueTrigger
{
    protected override void OnTriggerDialogue(InputAction.CallbackContext context)
    {
        if (_isInRange && !DialogueManager.Instance.isTalking) {
            // Fully Heal player
            _player.GetComponent<PlayerHealth>().FullHeal();

            // Manually Save
            SaveManager.Save(_player);
        }
    }
}