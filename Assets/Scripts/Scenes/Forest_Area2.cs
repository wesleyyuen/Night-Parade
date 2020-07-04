using UnityEngine;

public class Forest_Area2 : MonoBehaviour {
    public Transform player;
    public Transform area1SpawnPoint;
    public Transform treeAreaSpawnPoint;
    void Start () {
        GameMaster gameMaster = FindObjectOfType<GameMaster> ();
        gameMaster.UpdateCurrentScene ();

        if (gameMaster.GetPrevScene () == "Forest_Area1") {
            player.position = area1SpawnPoint.position;
            player.localScale = new Vector3 (1f, 1f, 1f);
        } else if (gameMaster.GetPrevScene () == "Forest_Tree") {
            player.position = treeAreaSpawnPoint.position;
            player.localScale = new Vector3 (-1f, 1f, 1f);
        }

        GetComponent<Parallax> ().FixParallaxOnLoad ();
    }
}