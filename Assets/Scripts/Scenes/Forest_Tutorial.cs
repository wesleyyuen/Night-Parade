using UnityEngine;

public class Forest_Tutorial : MonoBehaviour {
    [SerializeField] private Transform player;
    [SerializeField] private Transform area1SpawnPoint;
    void Start () {
        GameMaster gameMaster = FindObjectOfType<GameMaster> ();
        gameMaster.UpdateCurrentScene ();

        if (gameMaster.GetPrevScene () == "Forest_Area1") {
            player.position = area1SpawnPoint.position;
            player.localScale = new Vector3 (-1f, 1f, 1f);
            // 83,44,120
        }
    }
}