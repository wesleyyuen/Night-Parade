using UnityEngine;

public class Forest_Area1 : MonoBehaviour {
    [SerializeField] private Transform player;
    [SerializeField] private Transform tutorialAreaSpawnPoint;
    [SerializeField] private Transform area2SpawnPoint;
    void Start () {
        GameMaster gameMaster = FindObjectOfType<GameMaster> ();
        gameMaster.UpdateCurrentScene ();

        if (gameMaster.GetPrevScene () == "Forest_Tutorial") { 
            player.position = tutorialAreaSpawnPoint.position;
            player.localScale = new Vector3 (1f, 1f, 1f);
        } else if (gameMaster.GetPrevScene () == "Forest_Area2") {
            player.position = area2SpawnPoint.position;
            player.localScale = new Vector3 (-1f, 1f, 1f);
        }
    }
}