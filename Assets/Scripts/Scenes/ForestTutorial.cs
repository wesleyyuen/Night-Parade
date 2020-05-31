using UnityEngine;

public class ForestTutorial : MonoBehaviour {
    public Transform player;
    public Transform area1SpawnPoint;
    public Transform area2SpawnPoint;
    void Start () {
        GameMaster gameMaster = FindObjectOfType<GameMaster> ();
        gameMaster.UpdateCurrentScene ();

        if (gameMaster.GetPrevScene () == "Forest_Area1" || gameMaster.GetPrevScene () == "Forest_Area2") {
            foreach (GameObject obs in GameObject.FindGameObjectsWithTag ("BreakableObstacle")) {
                obs.SetActive (false);
            }
        }
        if (gameMaster.GetPrevScene () == "Forest_Area1") {
            player.position = area1SpawnPoint.position;
            player.localScale = new Vector3 (-1f, 1f, 1f);
            // 83,44,120
        } else if (gameMaster.GetPrevScene () == "Forest_Area2") {
            player.position = area2SpawnPoint.position;
            player.localScale = new Vector3 (-1f, 1f, 1f);
            //83,50.5,120
        }

        GetComponent<Parallax> ().FixParallaxOnLoad ();
    }
}