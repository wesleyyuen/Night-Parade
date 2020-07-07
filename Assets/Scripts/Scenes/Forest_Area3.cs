using UnityEngine;

public class Forest_Area3 : MonoBehaviour {
    [SerializeField] private Transform player;
    [SerializeField] private Transform treeAreaSpawnPoint;
    [SerializeField] private Transform hatsumuraSpawnPoint;
    void Start () {
        GameMaster gameMaster = FindObjectOfType<GameMaster> ();
        gameMaster.UpdateCurrentScene ();

        if (gameMaster.GetPrevScene () == "Forest_Tree") {
            player.position = treeAreaSpawnPoint.position;
            player.localScale = new Vector3 (1f, 1f, 1f);
        } else if (gameMaster.GetPrevScene () == "Hatsumura") {
            player.position = hatsumuraSpawnPoint.position;
            player.localScale = new Vector3 (-1f, 1f, 1f);
        }
    }
}