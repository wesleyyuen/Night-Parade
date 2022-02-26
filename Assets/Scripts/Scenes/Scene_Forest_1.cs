using UnityEngine;

public class Scene_Forest_1 : SceneInstaller
{
    private enum SpawnPoint {
        NotTesting,
        Cave,
        Save
    }
    [SerializeField] private AudioEvent _ambient;
    [SerializeField] private SpawnPoint spawnPoint;
    [SerializeField] private Transform caveSpawnPoint;
    [SerializeField] private Transform saveSpawnPoint;
    protected override void Start()
    {
        base.Start();
        
        // Handle BGM
        if (GameMaster.Instance.prevScene != "Forest_Cave") {
            _ambient.Play();
        }

        if (GameMaster.Instance.prevScene == "Main_Menu"
            || spawnPoint == SpawnPoint.Save) {
            foreach (Transform child in _playerGroup) {
                child.position = saveSpawnPoint.position;
                if (child.name == "Player")
                    child.GetComponent<PlayerAnimations>().FaceRight(true);
                else if (child.name == "Onibi")
                    child.position = saveSpawnPoint.position + new Vector3(-1.5f, 4f, 0f);
            }
        }
        else if (GameMaster.Instance.prevScene == "Forest_Cave"
            || spawnPoint == SpawnPoint.Cave) {
            foreach (Transform child in _playerGroup) {
                child.position = caveSpawnPoint.position;
                if (child.name == "Player")
                    child.GetComponent<PlayerAnimations>().FaceRight(true);
                else if (child.name == "Onibi")
                    child.position = caveSpawnPoint.position + new Vector3(-1.5f, 4f, 0f);
            }
        }
    }
}