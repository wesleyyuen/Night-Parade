using UnityEngine;

public class Forest_Area2 : MonoBehaviour {
    [SerializeField] private Transform playerGroup;
    [SerializeField] private Transform treeAreaSpawnPoint;
    [SerializeField] private Transform area3SpawnPoint;
    [SerializeField] private Transform area4SpawnPoint;
    void Start () {
        GameMaster.Instance.UpdateCurrentScene ();

        if (GameMaster.Instance.prevScene == "Forest_Tree") {
            foreach (Transform child in playerGroup) {
                child.position = treeAreaSpawnPoint.position;
                if (child.name == "Player")
                    child.localScale = new Vector3 (1f, 1f, 1f);
            }
        } else if (GameMaster.Instance.prevScene == "Forest_Area3") {
            foreach (Transform child in playerGroup) {
                child.position = area3SpawnPoint.position;
                if (child.name == "Player")
                    child.localScale = new Vector3 (-1f, 1f, 1f);
            }
        } else if (GameMaster.Instance.prevScene == "Forest_Area4") {
            foreach (Transform child in playerGroup) {
                child.position = area4SpawnPoint.position;
                if (child.name == "Player")
                    child.localScale = new Vector3 (1f, 1f, 1f);
            }
        }
    }
}