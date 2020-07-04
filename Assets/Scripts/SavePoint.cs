using UnityEngine;

public class SavePoint : DialogueTrigger {

    public override void Update () {
        bool isTalking = FindObjectOfType<DialogueManager> ().isTalking;
        int loadIndex = FindObjectOfType<GameMaster> ().savedPlayerData.SaveFileIndex;
        if (!isTalking && Vector2.Distance (player.transform.position, transform.position) <= triggerRange) {
            textPrompt.enabled = true;
            if (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.DownArrow)) {
                TriggerDialogue ();
                FindObjectOfType<PlayerHealth> ().FullHeal ();
                SaveManager.Save (player);
            }
        } else {
            textPrompt.enabled = false;
        }
    }
}