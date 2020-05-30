using UnityEngine;

public class Forest_Sacred : MonoBehaviour
{
    public Transform player;
    public Transform area1SpawnPoint;
    public Transform area2SpawnPoint;
    void Start () {
        GameMaster gameMaster = FindObjectOfType<GameMaster> ();
        gameMaster.UpdateCurrentScene ();
        Transform background = GameObject.FindGameObjectWithTag("Parallax").transform;
        
        if (gameMaster.getPrevScene () == "Forest_Area1") {
            player.position = area1SpawnPoint.position;
            player.localScale = new Vector3 (1f, 1f, 1f);
            // 0,0,120
        }
        else if (gameMaster.getPrevScene() == "Forest_Area2") {
            player.position = area2SpawnPoint.position;
            player.localScale = new Vector3 (1f, 1f, 1f);
        }

        GetComponent<Parallax>().FixParallaxOnLoad();
    }
}
