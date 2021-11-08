using TMPro;
using UnityEngine;

public class Scene_Forest_Cave : SceneScript
{
    enum SpawnPoint {
        NotTesting,
        Forest_1
    }
    [SerializeField] SpawnPoint _spawnPoint;
    [SerializeField] Transform _forest1SpawnPoint;
    [SerializeField] TextMeshProUGUI forestText;
    
    protected override void Start()
    {
        base.Start();

        // Handle BGM
        if (GameMaster.Instance.prevScene != "Forest_1")
            SoundManager.Instance.Play("Forest_Ambience");

        if (GameMaster.Instance.prevScene == "Main_Menu") {
            _spawnPoint = SpawnPoint.NotTesting;
            // Show Area Text
            Utility.FadeAreaText(forestText);
        }
        else if (GameMaster.Instance.prevScene == "Forest_1"
            || _spawnPoint == SpawnPoint.Forest_1) {
            foreach (Transform child in _playerGroup) {
                child.position = _forest1SpawnPoint.position;
                if (child.name == "Player")
                    child.GetComponent<PlayerAnimations>().FaceRight(false);
                else if (child.name == "Onibi")
                    child.position = _forest1SpawnPoint.position + new Vector3(1.5f, 4f, 0f);
            }
        }
    }
}
