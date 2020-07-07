using UnityEngine;

public class Forest_Area2 : MonoBehaviour {
    [SerializeField] private Transform player;
    [SerializeField] private Transform area1SpawnPoint;
    [SerializeField] private Transform treeAreaSpawnPoint;
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
    }
}