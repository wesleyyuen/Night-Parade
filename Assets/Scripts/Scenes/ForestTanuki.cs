using UnityEngine;

public class ForestTanuki : MonoBehaviour {
    public Transform player;

    // Use this for initialization
    void Start () {
        GameMaster gameMaster = FindObjectOfType<GameMaster> ();
        gameMaster.UpdateCurrentScene ();

        if (gameMaster.getPrevScene () == "Forest_Main") {
            player.position = new Vector2 (-59f, 8.5f);
        }
    }
}