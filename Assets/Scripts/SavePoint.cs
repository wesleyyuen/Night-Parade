using UnityEngine;

public class SavePoint : DialogueTrigger
{
    public override void Update ()
    {
        bool isTalking = DialogueManager.Instance.isTalking;
        int loadIndex = GameMaster.Instance.savedPlayerData.SaveFileIndex;

        // Show save prompt
        if (!isTalking && Vector2.Distance (player.transform.position, transform.position) <= triggerRange) {
            textPrompt.enabled = true;
            if (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.DownArrow)) {
                TriggerDialogue ();

                // Fully Heal player
                FindObjectOfType<PlayerHealth> ().FullHeal ();

                // Manually Save
                SaveManager.Save (player);
            }
        } else {
            textPrompt.enabled = false;
        }
    }
}