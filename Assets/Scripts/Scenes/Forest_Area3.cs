using UnityEngine;

public class Forest_Area3 : MonoBehaviour {
    public Transform player;
    public Transform sacredAreaSpawnPoint;
    void Start () {
        GameMaster gameMaster = FindObjectOfType<GameMaster> ();
        gameMaster.UpdateCurrentScene ();

        if (gameMaster.GetPrevScene () == "Forest_Sacred") {
            player.position = sacredAreaSpawnPoint.position;
            player.localScale = new Vector3 (1f, 1f, 1f);
        }

        GetComponent<Parallax> ().FixParallaxOnLoad ();
    }
}