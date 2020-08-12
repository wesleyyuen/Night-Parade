using UnityEngine;

public class Forest_Area2 : MonoBehaviour {
    [SerializeField] private Transform player;
    [SerializeField] private Transform treeAreaSpawnPoint;
    [SerializeField] private Transform area3SpawnPoint;
    [SerializeField] private Transform area4SpawnPoint;
    void Start () {
        GameMaster gameMaster = FindObjectOfType<GameMaster> ();
        gameMaster.UpdateCurrentScene ();

        if (gameMaster.GetPrevScene () == "Forest_Tree") {
            player.position = treeAreaSpawnPoint.position;
            player.localScale = new Vector3 (1f, 1f, 1f);
        } else if (gameMaster.GetPrevScene () == "Forest_Area3") {
            player.position = area3SpawnPoint.position;
            player.localScale = new Vector3 (-1f, 1f, 1f);
        } else if (gameMaster.GetPrevScene () == "Forest_Area4") {
            player.position = area4SpawnPoint.position;
            player.localScale = new Vector3 (1f, 1f, 1f);
        }
    }
}