using UnityEngine;

public class SavePoint : DialogueTrigger {

    public override void Update () {
        bool isTalking = FindObjectOfType<DialogueManager> ().isTalking;
        int loadIndex = FindObjectOfType<GameMaster> ().savedPlayerData.SaveFileIndex;
        if (!isTalking && Vector2.Distance (player.transform.position, transform.position) <= triggerRange && Input.GetKeyDown (KeyCode.DownArrow)) {
            TriggerDialogue ();
            SaveManager.Save (player);
        }
    }
}