using UnityEngine;

public class Forest_Area1 : MonoBehaviour {
    [SerializeField] private Transform playerGroup;
    [SerializeField] private Transform tutorialAreaSpawnPoint;
    [SerializeField] private Transform treeAreaSpawnPoint;
    void Start () {
        GameMaster.Instance.UpdateCurrentScene ();

        if (GameMaster.Instance.GetPrevScene () == "Forest_Tutorial") {
            foreach (Transform child in playerGroup) {
                child.position = tutorialAreaSpawnPoint.position;
                if (child.name == "Player")
                    child.localScale = new Vector3 (1f, 1f, 1f);
            }
        }
        else if (GameMaster.Instance.GetPrevScene () == "Forest_Tree") {
            foreach (Transform child in playerGroup) {
                child.position = treeAreaSpawnPoint.position;
                if (child.name == "Player")
                    child.localScale = new Vector3 (-1f, 1f, 1f);
            }
        }
    }
}