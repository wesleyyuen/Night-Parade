using TMPro;
using UnityEngine;

public class Forest_Cave : MonoBehaviour
{
    enum SpawnPoint {
        NotTesting,
        Area1
    }
    [SerializeField] SpawnPoint spawnPoint;
    [SerializeField] Transform playerGroup;
    [SerializeField] Transform area1SpawnPoint;
    [SerializeField] TextMeshProUGUI forestText;
    
    void Start ()
    {
        GameMaster.Instance.UpdateCurrentScene ();

        if (GameMaster.Instance.prevScene == "Main_Menu") {
            spawnPoint = SpawnPoint.NotTesting;
            Utility.FadeAreaText(forestText);
        }

        if (GameMaster.Instance.prevScene == "Forest_Area1"
            || spawnPoint == SpawnPoint.Area1) {
            foreach (Transform child in playerGroup) {
                child.position = area1SpawnPoint.position;
                if (child.name == "Player")
                    child.localScale = new Vector3 (-1f, 1f, 1f);
            }
        }
    }
}
