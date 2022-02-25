public class SavePoint : DialogueTrigger
{
    protected override void OnTriggerDialogue()
    {
        if (_isInRange && !DialogueManager.Instance.isTalking) {
            // Fully Heal player
            _player.GetComponent<PlayerHealthMO>().FullHeal();

            // Manually Save
            if (SaveManager.Save(_player)) {
                OnSaveComplete();
            } else {
                OnSaveFailed();
            }
        }
    }
    private void OnSaveComplete()
    {
        
    }

    private void OnSaveFailed()
    {

    }
}