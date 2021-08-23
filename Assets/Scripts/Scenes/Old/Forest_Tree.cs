using UnityEngine;
using UnityEngine.Playables;

public class Forest_Tree : MonoBehaviour {
    [SerializeField] private Transform playerGroup;
    [SerializeField] private Transform savePointSpawn;

    [SerializeField] private Transform area1SpawnPoint;
    [SerializeField] private Transform area2SpawnPoint;
    [SerializeField] private Transform rootSpawnPoint;
    [SerializeField] private PlayableDirector rootSpawnAnimation;
    void Start () {
        GameMaster.Instance.UpdateCurrentScene ();

        // TODO: should be "Main_Menu" instead of "Main_Menu", but _Preload is loaded for a frame for some reason
        if (GameMaster.Instance.prevScene == "_Preload" || GameMaster.Instance.prevScene == "Main_Menu") { 
            foreach (Transform child in playerGroup) {
                child.position = savePointSpawn.position;
                if (child.name == "Player")
                    child.localScale = new Vector3 (1f, 1f, 1f);
            }
        }
        else if (GameMaster.Instance.prevScene == "Forest_Area1") {
            foreach (Transform child in playerGroup) {
                child.position = area1SpawnPoint.position;
                if (child.name == "Player")
                    child.localScale = new Vector3 (1f, 1f, 1f);
            }
        }
        else if (GameMaster.Instance.prevScene == "Forest_Area2") {
            foreach (Transform child in playerGroup) {
                child.position = area2SpawnPoint.position;
                if (child.name == "Player")
                    child.localScale = new Vector3 (-1f, 1f, 1f);
            }
        }
        else if (GameMaster.Instance.prevScene == "Forest_Root") {
            foreach (Transform child in playerGroup) {
                child.position = rootSpawnPoint.position;
                if (child.name == "Player")
                    child.localScale = new Vector3 (1f, 1f, 1f);
            }
            rootSpawnAnimation.Play();
        }
    }
}