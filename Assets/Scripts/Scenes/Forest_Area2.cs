using UnityEngine;

public class Forest_Area2 : MonoBehaviour {
    public Transform player;
    public Transform tutorialAreaSpawnPoint;
    public Transform sacredAreaSpawnPoint;
    void Start () {
        GameMaster gameMaster = FindObjectOfType<GameMaster> ();
        gameMaster.UpdateCurrentScene ();

        if (gameMaster.GetPrevScene () == "Forest_Tutorial") {
            player.position = tutorialAreaSpawnPoint.position;
            player.localScale = new Vector3 (1f, 1f, 1f);
        } else if (gameMaster.GetPrevScene () == "Forest_Sacred") {
            player.position = sacredAreaSpawnPoint.position;
            player.localScale = new Vector3 (-1f, 1f, 1f);
        }

        GetComponent<Parallax> ().FixParallaxOnLoad ();
    }
}