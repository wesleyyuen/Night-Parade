using UnityEngine;
using UnityEngine.Playables;

public class Forest_Tree : MonoBehaviour {
    [SerializeField] private Transform player;
    [SerializeField] private Transform savePointSpawn;

    [SerializeField] private Transform area2SpawnPoint;
    [SerializeField] private Transform area3SpawnPoint;
    [SerializeField] private Transform rootSpawnPoint;
    [SerializeField] private PlayableDirector rootSpawnAnimation;
    void Start () {
        GameMaster gameMaster = FindObjectOfType<GameMaster> ();
        gameMaster.UpdateCurrentScene ();
        Debug.Log(gameMaster.GetPrevScene ());
        if (gameMaster.GetPrevScene () == "_Preload" || gameMaster.GetPrevScene () == "Main_Menu") { // TODO should be "Main_Menu" instead of "Main_Menu", but _Preload is loaded for a frame for some reason
            player.position = savePointSpawn.position;
            player.localScale = new Vector3 (1f, 1f, 1f);
        } else if (gameMaster.GetPrevScene () == "Forest_Area2") {
            player.position = area2SpawnPoint.position;
            player.localScale = new Vector3 (1f, 1f, 1f);
        } else if (gameMaster.GetPrevScene () == "Forest_Area3") {
            player.position = area3SpawnPoint.position;
            player.localScale = new Vector3 (-1f, 1f, 1f);
        } else if (gameMaster.GetPrevScene () == "Forest_Root") {
            player.position = rootSpawnPoint.position;
            player.localScale = new Vector3 (1f, 1f, 1f);
            rootSpawnAnimation.Play();
        }
    }
}