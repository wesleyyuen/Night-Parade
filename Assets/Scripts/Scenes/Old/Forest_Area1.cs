using UnityEngine;

public class Forest_Area1 : MonoBehaviour {
    enum SpawnPoint {
        NotTesting,
        Cave
    }
    [SerializeField] SpawnPoint spawnPoint;
    [SerializeField] Transform playerGroup;
    [SerializeField] Transform caveSpawnPoint;
    void Start ()
    {
        GameMaster.Instance.UpdateCurrentScene ();

        if (GameMaster.Instance.prevScene == "Forest_Cave"
            || spawnPoint == SpawnPoint.Cave) {
            foreach (Transform child in playerGroup) {
                child.position = caveSpawnPoint.position;
                if (child.name == "Player")
                    child.localScale = new Vector3 (1f, 1f, 1f);
            }
        }
    }
}