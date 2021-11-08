using UnityEngine;

public class Scene_Forest_1 : SceneScript
{
    enum SpawnPoint {
        NotTesting,
        Cave,
        Save
    }
    [SerializeField] SpawnPoint spawnPoint;
    [SerializeField] Transform caveSpawnPoint;
    [SerializeField] Transform saveSpawnPoint;
    protected override void Start()
    {
        base.Start();
        
        // Handle BGM
        if (GameMaster.Instance.prevScene != "Forest_Cave")
            SoundManager.Instance.Play("Forest_Ambience");

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